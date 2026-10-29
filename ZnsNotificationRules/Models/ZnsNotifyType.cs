namespace ZnsNotificationRules.Models
{
    /// <summary>Gia tri NOTIFY_TYPE luu trong ZNS_TRANSACTION.</summary>
    public static class ZnsNotifyType
    {
        public const string Payment = "PAYMENT";
        public const string Approve = "APPROVE";
        public const string Reject = "REJECT";
        public const string Complete = "COMPLETE";

        // GIA DINH (placeholder) - chua co nghiep vu that
        public const string Cancel = "CANCEL";
    }
}
