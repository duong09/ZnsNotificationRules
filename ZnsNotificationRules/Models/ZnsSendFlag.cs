namespace ZnsNotificationRules.Models
{
    /// <summary>Gia tri SEND_FLG trong ZNS_TRANSACTION.</summary>
    public static class ZnsSendFlag
    {
        /// <summary>Cho gui.</summary>
        public const string Pending = "N";

        /// <summary>Da gui thanh cong.</summary>
        public const string Sent = "Y";

        /// <summary>Bo qua kem ly do (khong gui).</summary>
        public const string Skipped = "S";

        /// <summary>Gui that bai, het so lan thu lai.</summary>
        public const string Failed = "F";
    }
}
