using System;
using System.Collections.Generic;
using System.Linq;
using ZnsNotificationRules.Formatting;
using ZnsNotificationRules.Models;
using ZnsNotificationRules.Rules.Base;

namespace ZnsNotificationRules.Rules
{
    /// <summary>Tin PAYMENT: thong bao dong phi dao chuyen cat/bam seal.</summary>
    public sealed class PaymentZnsRule : ZnsNotificationRuleBase
    {
        private int _payExpiryHrs = ZnsConfigCodes.DefaultPayExpiryHrs;

        public PaymentZnsRule()
            : base(ZnsNotifyType.Payment)
        {
        }

        protected override ZnsTemplateLimits Limits => ZnsTemplateLimits.Payment;

        protected override string[] SpecificParams => new[] { ZnsSealParamName.PaymentExpiryTs };

        public override void LoadConfig(IZnsConfigReader config)
        {
            _payExpiryHrs = config.GetInt(ZnsConfigCodes.PayExpiryHrs, ZnsConfigCodes.DefaultPayExpiryHrs);
        }

        /// <summary>
        /// GIA LAP: ho so cho thanh toan. SQL that se loc theo TTDD xac nhan trong _payExpiryHrs gio.
        /// </summary>
        protected override IReadOnlyList<ZnsTransactionNotifyDto> FetchPending(ZnsRunContext context)
        {
            var samples = new[]
            {
                new ZnsTransactionNotifyDto
                {
                    KeyValue = "SIM-PAY-001",
                    OrderId = 301,
                    CustomerName = "CONG TY D (GIA LAP)",
                    ItemNo = "CONT-0301",
                    AuthorizedPhoneNo = "0900000301",
                    RequestTs = DateTime.Now.AddDays(-1),
                    TerOpConfirmedTs = DateTime.Now.AddHours(-1),
                    SourceEventTs = DateTime.Now.AddHours(-1)
                },
                // Chua co moc TTDD xac nhan, de thu nhanh bo qua
                new ZnsTransactionNotifyDto
                {
                    KeyValue = "SIM-PAY-002",
                    OrderId = 302,
                    CustomerName = "CONG TY E (GIA LAP)",
                    ItemNo = "CONT-0302",
                    AuthorizedPhoneNo = "0900000302",
                    RequestTs = DateTime.Now.AddDays(-1)
                }
            };

            return samples.Take(context.BatchSize).ToList();
        }

        protected override string GetSpecificSkipReason(ZnsTransactionNotifyDto item)
        {
            return ZnsText.IsNullDateTime(item.TerOpConfirmedTs)
                ? "Thieu moc TTDD xac nhan, khong tinh duoc han thanh toan"
                : string.Empty;
        }

        protected override void AddSpecificParams(IDictionary<string, string> templateData, ZnsTransactionNotifyDto item)
        {
            var expiryTs = item.TerOpConfirmedTs.AddHours(_payExpiryHrs);

            templateData[ZnsSealParamName.PaymentExpiryTs] =
                ZnsText.Truncate(ZnsText.FormatTs(expiryTs), Limits.SpecificParam);
        }
    }
}
