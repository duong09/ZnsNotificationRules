using System;
using System.Collections.Generic;
using System.Linq;
using ZnsNotificationRules.Formatting;
using ZnsNotificationRules.Models;
using ZnsNotificationRules.Rules.Base;

namespace ZnsNotificationRules.Rules
{
    /// <summary>
    /// Tin CANCEL (placeholder): cau truc da hoan chinh, nghiep vu va tham so chua xac nhan.
    /// Lam theo gia dinh: ly do huy lay tu Attributes[CancelReason], gioi han theo ZnsTemplateLimits.Cancel.
    /// </summary>
    public sealed class CancelZnsRule : ZnsNotificationRuleBase
    {
        public CancelZnsRule()
            : base(ZnsNotifyType.Cancel)
        {
        }

        protected override ZnsTemplateLimits Limits => ZnsTemplateLimits.Cancel;

        protected override string[] SpecificParams => new[] { ZnsSealParamName.CancelReason };

        /// <summary>GIA LAP (placeholder): ho so bi huy. Nguon that chua xac nhan.</summary>
        protected override IReadOnlyList<ZnsTransactionNotifyDto> FetchPending(ZnsRunContext context)
        {
            var sample = new ZnsTransactionNotifyDto
            {
                KeyValue = "SIM-CAN-001",
                OrderId = 501,
                CustomerName = "CONG TY G (GIA LAP)",
                ItemNo = "CONT-0501",
                AuthorizedPhoneNo = "0900000501",
                RequestTs = DateTime.Now.AddDays(-1),
                SourceEventTs = DateTime.Now.AddMinutes(-20)
            };
            sample.Attributes[ZnsAttributeKeys.CancelReason] = "Khach huy (GIA LAP)";

            return new[] { sample }.Take(context.BatchSize).ToList();
        }

        protected override void AddSpecificParams(IDictionary<string, string> templateData, ZnsTransactionNotifyDto item)
        {
            var reason = item.GetAttribute(ZnsAttributeKeys.CancelReason);

            templateData[ZnsSealParamName.CancelReason] = ZnsText.Truncate(reason, Limits.SpecificParam);
        }
    }
}
