using System;
using System.Collections.Generic;
using System.Linq;
using ZnsNotificationRules.Formatting;
using ZnsNotificationRules.Models;
using ZnsNotificationRules.Rules.Base;

namespace ZnsNotificationRules.Rules
{
    /// <summary>Tin REJECT: thong bao tu choi cat/bam seal kem ly do.</summary>
    public sealed class RejectZnsRule : ZnsNotificationRuleBase
    {
        public RejectZnsRule()
            : base(ZnsNotifyType.Reject)
        {
        }

        protected override ZnsTemplateLimits Limits => ZnsTemplateLimits.Reject;

        protected override string[] SpecificParams => new[] { ZnsSealParamName.RejectRegion };

        /// <summary>GIA LAP: ho so bi tu choi. SQL that se loc theo trang thai tu choi chua gui REJECT.</summary>
        protected override IReadOnlyList<ZnsTransactionNotifyDto> FetchPending(ZnsRunContext context)
        {
            var samples = new[]
            {
                new ZnsTransactionNotifyDto
                {
                    KeyValue = "SIM-REJ-001",
                    OrderId = 401,
                    CustomerName = "CONG TY F (GIA LAP)",
                    ItemNo = "CONT-0401",
                    AuthorizedPhoneNo = "0900000401",
                    RequestTs = DateTime.Now.AddDays(-1),
                    RejectReason = "Thieu giay to hai quan (GIA LAP)",
                    SourceEventTs = DateTime.Now.AddMinutes(-20)
                }
            };

            return samples.Take(context.BatchSize).ToList();
        }

        protected override void AddSpecificParams(IDictionary<string, string> templateData, ZnsTransactionNotifyDto item)
        {
            templateData[ZnsSealParamName.RejectRegion] = ZnsText.Truncate(item.RejectReason, Limits.SpecificParam);
        }
    }
}
