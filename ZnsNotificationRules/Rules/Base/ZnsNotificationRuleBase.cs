using System;
using System.Collections.Generic;
using System.Linq;
using ZnsNotificationRules.Contracts;
using ZnsNotificationRules.Formatting;
using ZnsNotificationRules.Models;

namespace ZnsNotificationRules.Rules.Base
{
    /// <summary>
    /// Phan dung chung cua moi loai tin (Template Method).
    /// Quy trinh co dinh, khong override. Class con dien cac diem mo rong:
    /// FetchPending (truy van va tong hop ho so rieng), Limits, SpecificParams, AddSpecificParams,
    /// GetSpecificSkipReason, LoadConfig.
    /// </summary>
    public abstract class ZnsNotificationRuleBase : IZnsNotificationRule
    {
        private static readonly string[] CommonParams =
        {
            ZnsSealParamName.CustomerName,
            ZnsSealParamName.ContainerNo,
            ZnsSealParamName.RequestTs
        };

        protected ZnsNotificationRuleBase(string notifyType)
        {
            if (string.IsNullOrWhiteSpace(notifyType))
                throw new ArgumentException("NotifyType is required.", nameof(notifyType));

            NotifyType = notifyType;
        }

        public string NotifyType { get; }

        public IReadOnlyCollection<string> RequiredParams =>
            CommonParams.Concat(SpecificParams).ToList();

        /// <summary>Gioi han do dai cua template nay.</summary>
        protected abstract ZnsTemplateLimits Limits { get; }

        /// <summary>Ten tham so rieng cua template (ngoai tham so chung).</summary>
        protected abstract string[] SpecificParams { get; }

        public virtual void LoadConfig(IZnsConfigReader config)
        {
        }

        public IReadOnlyList<ZnsTransactionNotifyDto> FindPending(ZnsRunContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var items = FetchPending(context) ?? new ZnsTransactionNotifyDto[0];

            foreach (var item in items)
            {
                item.NotifyType = NotifyType;
            }

            return items;
        }

        public IReadOnlyList<ZnsQueueEntry> BuildPendingEntries(ZnsRunContext context, IZnsRecipientResolver recipients)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (recipients == null)
                throw new ArgumentNullException(nameof(recipients));

            return FindPending(context)
                .Select(item => ToQueueEntry(context, item, recipients))
                .ToList();
        }

        public string GetSkipReason(ZnsTransactionNotifyDto item, string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return "Khong tim duoc so dien thoai hop le";

            if (item.OrderId <= 0)
                return "Thieu thong tin ma don hang Eport (CUSTOMS_SEAL_CUTTING.ORDER_ID <= 0)";

            if (ZnsText.IsNullDateTime(item.RequestTs))
                return "Thieu ngay khach tao dang ky tren Eport (CUSTOMS_SEAL_CUTTING.CREATED_ORDER_TS)";

            return GetSpecificSkipReason(item);
        }

        public IDictionary<string, string> BuildTemplateData(ZnsTransactionNotifyDto item)
        {
            var templateData = new Dictionary<string, string>
            {
                { ZnsSealParamName.CustomerName, ZnsText.Truncate(item.CustomerName, Limits.CustomerName) },
                { ZnsSealParamName.ContainerNo, ZnsText.Truncate(item.ItemNo, Limits.ContainerNo) },
                { ZnsSealParamName.RequestTs, ZnsText.Truncate(ZnsText.FormatTs(item.RequestTs), Limits.RequestTs) }
            };

            AddSpecificParams(templateData, item);

            return templateData;
        }

        public IReadOnlyList<string> GetMissingParams(IDictionary<string, string> templateData)
        {
            if (templateData == null)
                return RequiredParams.ToList();

            return RequiredParams
                .Where(key => !templateData.TryGetValue(key, out var value) || string.IsNullOrEmpty(value))
                .ToList();
        }

        /// <summary>
        /// Truy van va tong hop ho so rieng cua loai tin. Moi rule tu implement theo logic cua minh.
        /// Ket qua da chon theo context.BatchSize.
        /// </summary>
        protected abstract IReadOnlyList<ZnsTransactionNotifyDto> FetchPending(ZnsRunContext context);

        /// <summary>Validate rieng cua loai tin. Mac dinh khong co.</summary>
        protected virtual string GetSpecificSkipReason(ZnsTransactionNotifyDto item)
        {
            return string.Empty;
        }

        /// <summary>Them tham so rieng cua loai tin vao tui.</summary>
        protected abstract void AddSpecificParams(IDictionary<string, string> templateData, ZnsTransactionNotifyDto item);

        private ZnsQueueEntry ToQueueEntry(ZnsRunContext context, ZnsTransactionNotifyDto item, IZnsRecipientResolver recipients)
        {
            var phone = recipients.ResolvePhone(item);

            var skipReason = GetSkipReason(item, phone);
            if (!string.IsNullOrEmpty(skipReason))
                return ZnsQueueEntry.ForSkip(context.NotificationGroup, NotifyType, item, phone, skipReason);

            var templateData = BuildTemplateData(item);
            var missingParams = GetMissingParams(templateData);
            if (missingParams.Count > 0)
                return ZnsQueueEntry.ForSkip(context.NotificationGroup, NotifyType, item, phone, "Thieu tham so: " + string.Join(", ", missingParams));

            return ZnsQueueEntry.ForSend(context.NotificationGroup, NotifyType, item, phone, templateData);
        }
    }
}
