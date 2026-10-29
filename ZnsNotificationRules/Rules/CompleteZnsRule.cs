using System;
using System.Collections.Generic;
using System.Linq;
using ZnsNotificationRules.Contracts;
using ZnsNotificationRules.Formatting;
using ZnsNotificationRules.Models;
using ZnsNotificationRules.Rules.Base;

namespace ZnsNotificationRules.Rules
{
    /// <summary>Tin COMPLETE: thong bao hoan tat cat/bam seal tai bai, sau khi go stop DKS N phut.</summary>
    public sealed class CompleteZnsRule : ZnsNotificationRuleBase
    {
        private int _waitMinutes = ZnsConfigCodes.DefaultCompleteWaitMinutes;

        public CompleteZnsRule()
            : base(ZnsNotifyType.Complete)
        {
        }

        protected override ZnsTemplateLimits Limits => ZnsTemplateLimits.Complete;

        protected override string[] SpecificParams => new[] { ZnsSealParamName.CompleteTs };

        public override void LoadConfig(IZnsConfigReader config)
        {
            _waitMinutes = Math.Max(0, config.GetInt(ZnsConfigCodes.CompleteWaitMinutes, ZnsConfigCodes.DefaultCompleteWaitMinutes));
        }

        /// <summary>
        /// GIA LAP: form da go stop DKS va da qua _waitMinutes phut.
        /// SQL that se loc: CLR_TS &lt;= SYSDATE - _waitMinutes/1440, khong con DKS hieu luc, chua gui COMPLETE.
        /// </summary>
        protected override IReadOnlyList<ZnsTransactionNotifyDto> FetchPending(ZnsRunContext context)
        {
            var cutoff = DateTime.Now.AddMinutes(-_waitMinutes);

            var samples = new[]
            {
                new ZnsTransactionNotifyDto
                {
                    KeyValue = "SIM-COM-001",
                    OrderId = 601,
                    CustomerName = "CONG TY H (GIA LAP)",
                    ItemNo = "CONT-0601",
                    AuthorizedPhoneNo = "0900000601",
                    RequestTs = DateTime.Now.AddDays(-1),
                    CompleteTs = DateTime.Now.AddMinutes(-30),
                    SourceEventTs = DateTime.Now.AddMinutes(-30)
                },
                // Chua du thoi gian cho, bi loc theo cutoff
                new ZnsTransactionNotifyDto
                {
                    KeyValue = "SIM-COM-002",
                    OrderId = 602,
                    CustomerName = "CONG TY I (GIA LAP)",
                    ItemNo = "CONT-0602",
                    AuthorizedPhoneNo = "0900000602",
                    RequestTs = DateTime.Now.AddDays(-1),
                    CompleteTs = DateTime.Now.AddMinutes(-5),
                    SourceEventTs = DateTime.Now.AddMinutes(-5)
                }
            };

            return samples
                .Where(item => item.CompleteTs <= cutoff)
                .Take(context.BatchSize)
                .ToList();
        }

        protected override string GetSpecificSkipReason(ZnsTransactionNotifyDto item)
        {
            return ZnsText.IsNullDateTime(item.CompleteTs)
                ? "Thieu thoi diem go stop DKS (ITEM_STOPS.CLR_TS)"
                : string.Empty;
        }

        protected override void AddSpecificParams(IDictionary<string, string> templateData, ZnsTransactionNotifyDto item)
        {
            templateData[ZnsSealParamName.CompleteTs] = ZnsText.Truncate(ZnsText.FormatTs(item.CompleteTs), Limits.SpecificParam);
        }
    }
}
