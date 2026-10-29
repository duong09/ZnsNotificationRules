using System;

namespace ZnsNotificationRules.Models
{
    /// <summary>Tham so chung cua mot lan chay job, truyen xuong moi rule.</summary>
    public sealed class ZnsRunContext
    {
        public ZnsRunContext(string notificationGroup, int batchSize)
        {
            if (string.IsNullOrWhiteSpace(notificationGroup))
                throw new ArgumentException("NotificationGroup is required.", nameof(notificationGroup));
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize));

            NotificationGroup = notificationGroup;
            BatchSize = batchSize;
        }

        /// <summary>Ma nghiep vu cha, ghi vao ZNS_TRANSACTION.NOTIFY_GROUP.</summary>
        public string NotificationGroup { get; }

        public int BatchSize { get; }
    }
}
