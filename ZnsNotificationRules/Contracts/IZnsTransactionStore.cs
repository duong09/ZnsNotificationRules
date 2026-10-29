using System.Collections.Generic;
using ZnsNotificationRules.Models;

namespace ZnsNotificationRules.Contracts
{
    /// <summary>
    /// Cong ghi hang doi ZNS_TRANSACTION. Adapter phia TOPO implement.
    /// Adapter chiu trach nhiem: serialize TemplateData thanh JSON, chong trung (khoa duy nhat), map cot.
    /// </summary>
    public interface IZnsTransactionStore
    {
        void Save(ZnsQueueEntry entry);

        IReadOnlyList<ZnsPendingMessage> GetPending(string notifyGroup, int batchSize);

        void MarkSent(long transactionId, string messageId);

        /// <summary>isFinal = true: SEND_FLG = F. false: giu N, cap nhat RETRY_COUNT de lan chay sau gui lai.</summary>
        void MarkFailed(long transactionId, int retryCount, bool isFinal, string errorMessage);
    }
}
