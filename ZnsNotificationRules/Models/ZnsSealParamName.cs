namespace ZnsNotificationRules.Models
{
    /// <summary>Ten tham so cua template ZNS phia Zalo (khop ten key trong template_data).</summary>
    public static class ZnsSealParamName
    {
        public const string CustomerName = "Customer_Name";
        public const string ContainerNo = "Container_No";
        public const string RequestTs = "Request_Ts";
        public const string PaymentExpiryTs = "Payment_ExpiryTs";
        public const string SealWorkerTeam = "Seal_Worker_Team";
        public const string RejectRegion = "Reject_Region";
        public const string CompleteTs = "Complete_Ts";

        // GIA DINH (placeholder) - ten tham so template 618058 chua xac nhan
        public const string CancelReason = "Cancel_Reason";
    }
}
