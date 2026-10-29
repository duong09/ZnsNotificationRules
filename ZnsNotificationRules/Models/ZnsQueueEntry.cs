using System;
using System.Collections.Generic;

namespace ZnsNotificationRules.Models
{
    /// <summary>Mot dong ghi vao ZNS_TRANSACTION: N (cho gui, kem TemplateData) hoac S (bo qua, kem ly do).</summary>
    public sealed class ZnsQueueEntry
    {
        private ZnsQueueEntry(
            string sendFlag,
            string notifyGroup,
            string notifyType,
            ZnsTransactionNotifyDto item,
            string phone,
            string skipReason,
            IDictionary<string, string> templateData)
        {
            SendFlag = sendFlag;
            NotifyGroup = notifyGroup;
            NotifyType = notifyType;
            KeyValue = item.KeyValue;
            SourceEventTs = item.SourceEventTs;
            Phone = phone;
            SkipReason = skipReason;
            TemplateData = templateData;
        }

        public static ZnsQueueEntry ForSend(
            string notifyGroup,
            string notifyType,
            ZnsTransactionNotifyDto item,
            string phone,
            IDictionary<string, string> templateData)
        {
            return new ZnsQueueEntry(ZnsSendFlag.Pending, notifyGroup, notifyType, item, phone, string.Empty, templateData);
        }

        public static ZnsQueueEntry ForSkip(
            string notifyGroup,
            string notifyType,
            ZnsTransactionNotifyDto item,
            string phone,
            string skipReason)
        {
            return new ZnsQueueEntry(ZnsSendFlag.Skipped, notifyGroup, notifyType, item, phone, skipReason, null);
        }

        public string SendFlag { get; }
        public string NotifyGroup { get; }
        public string NotifyType { get; }
        public string KeyValue { get; }
        public DateTime SourceEventTs { get; }
        public string Phone { get; }
        public string SkipReason { get; }
        public IDictionary<string, string> TemplateData { get; }
    }
}
