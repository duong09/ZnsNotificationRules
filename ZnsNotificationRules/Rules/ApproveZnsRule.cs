using System;
using System.Collections.Generic;
using System.Linq;
using ZnsNotificationRules.Formatting;
using ZnsNotificationRules.Models;
using ZnsNotificationRules.Rules.Base;

namespace ZnsNotificationRules.Rules
{
    /// <summary>Tin APPROVE: thong bao duyet cat/bam seal, kem doi cong nhan.</summary>
    public sealed class ApproveZnsRule : ZnsNotificationRuleBase
    {
        public ApproveZnsRule()
            : base(ZnsNotifyType.Approve)
        {
        }

        protected override ZnsTemplateLimits Limits => ZnsTemplateLimits.Approve;

        protected override string[] SpecificParams => new[] { ZnsSealParamName.SealWorkerTeam };

        /// <summary>GIA LAP: ho so duoc duyet. SQL that se loc theo trang thai duyet va chua gui APPROVE.</summary>
        protected override IReadOnlyList<ZnsTransactionNotifyDto> FetchPending(ZnsRunContext context)
        {
            var samples = new[]
            {
                new ZnsTransactionNotifyDto
                {
                    KeyValue = "SIM-APP-001",
                    OrderId = 201,
                    CustomerName = "CONG TY C (GIA LAP)",
                    ItemNo = "CONT-0201",
                    AuthorizedPhoneNo = "0900000201",
                    RequestTs = DateTime.Now.AddDays(-1),
                    WorkerTeamName = "Doi 1",
                    WorkerTeamContact = "0911000001",
                    SourceEventTs = DateTime.Now.AddMinutes(-20)
                }
            };

            return samples.Take(context.BatchSize).ToList();
        }

        protected override void AddSpecificParams(IDictionary<string, string> templateData, ZnsTransactionNotifyDto item)
        {
            var workerTeam = ZnsText.BuildWorkerTeamText(item.WorkerTeamName, item.WorkerTeamContact);

            templateData[ZnsSealParamName.SealWorkerTeam] = ZnsText.Truncate(workerTeam, Limits.SpecificParam);
        }
    }
}
