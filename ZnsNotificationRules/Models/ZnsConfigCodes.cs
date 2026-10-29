namespace ZnsNotificationRules.Models
{
    /// <summary>Ma CODE_TP trong SYS_CODES va gia tri mac dinh khi thieu cau hinh.</summary>
    public static class ZnsConfigCodes
    {
        public const string PayExpiryHrs = "ZNSPAYHRS";
        public const string CompleteWaitMinutes = "ZNSCOMPMIN";
        public const string MaxRetry = "ZNSMAXRETRY";

        public const int DefaultPayExpiryHrs = 2;
        public const int DefaultCompleteWaitMinutes = 15;

        // GIA DINH - chua xac nhan gia tri mac dinh ZNSMAXRETRY voi nghiep vu
        public const int DefaultMaxRetry = 3;
    }
}
