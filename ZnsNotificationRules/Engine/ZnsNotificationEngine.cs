using System;
using System.Collections.Generic;
using System.Linq;
using ZnsNotificationRules.Contracts;
using ZnsNotificationRules.Models;

namespace ZnsNotificationRules.Engine
{
    /// <summary>
    /// Dieu phoi dung chung cho moi loai tin: ghi hang doi, gui.
    /// Khong biet loai tin cu the nao; viec rieng nam trong IZnsNotificationRule va cac cong (port).
    /// </summary>
    public sealed class ZnsNotificationEngine
    {
        private readonly IReadOnlyList<IZnsNotificationRule> _rules;
        private readonly IZnsConfigReader _config;
        private readonly IZnsRecipientResolver _recipients;
        private readonly IZnsTransactionStore _store;
        private readonly IZnsGateway _gateway;

        public ZnsNotificationEngine(
            IEnumerable<IZnsNotificationRule> rules,
            IZnsConfigReader config,
            IZnsRecipientResolver recipients,
            IZnsTransactionStore store,
            IZnsGateway gateway)
        {
            _rules = (rules ?? throw new ArgumentNullException(nameof(rules))).ToList();
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _recipients = recipients ?? throw new ArgumentNullException(nameof(recipients));
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
        }

        /// <summary>Buoc 1: moi rule tu tong hop ho so va ghi vao ZNS_TRANSACTION (N = gui, S = bo qua).</summary>
        public void Enqueue(ZnsRunContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            foreach (var rule in _rules)
            {
                rule.LoadConfig(_config);

                foreach (var entry in rule.BuildPendingEntries(context, _recipients))
                {
                    _store.Save(entry);
                }
            }
        }

        /// <summary>Buoc 2: gui cac dong N. Thanh cong -> Y. That bai -> giu N, tang RETRY_COUNT; dat ZNSMAXRETRY -> F.</summary>
        public void Dispatch(ZnsRunContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var maxRetry = _config.GetInt(ZnsConfigCodes.MaxRetry, ZnsConfigCodes.DefaultMaxRetry);

            foreach (var message in _store.GetPending(context.NotificationGroup, context.BatchSize))
            {
                Send(message, maxRetry);
            }
        }

        private void Send(ZnsPendingMessage message, int maxRetry)
        {
            var outbound = new ZnsOutboundMessage(message.NotifyType, message.Phone, message.TemplateData);
            var result = _gateway.Send(outbound);

            if (result.IsSuccess)
            {
                _store.MarkSent(message.TransactionId, result.MessageId);
                return;
            }

            var retryCount = message.RetryCount + 1;
            _store.MarkFailed(message.TransactionId, retryCount, retryCount >= maxRetry, result.ErrorMessage);
        }
    }
}
