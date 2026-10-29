using System.Collections.Generic;

namespace ZnsNotificationRules.Models
{
    /// <summary>Dong dang cho gui (SEND_FLG = N) do cong ZNS_TRANSACTION tra ve.</summary>
    public sealed class ZnsPendingMessage
    {
        public ZnsPendingMessage(
            long transactionId,
            string notifyType,
            string phone,
            IDictionary<string, string> templateData,
            int retryCount)
        {
            TransactionId = transactionId;
            NotifyType = notifyType;
            Phone = phone;
            TemplateData = templateData;
            RetryCount = retryCount;
        }

        public long TransactionId { get; }
        public string NotifyType { get; }
        public string Phone { get; }
        public IDictionary<string, string> TemplateData { get; }
        public int RetryCount { get; }
    }
}
