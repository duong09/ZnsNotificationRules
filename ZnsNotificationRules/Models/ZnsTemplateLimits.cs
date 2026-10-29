namespace ZnsNotificationRules.Models
{
    /// <summary>
    /// Gioi han do dai tung tham so cua mot template. Zalo tu choi ca tin neu tham so vuot do dai dang ky.
    /// </summary>
    public sealed class ZnsTemplateLimits
    {
        /// <summary>Template 618059 - thong bao dong phi.</summary>
        public static readonly ZnsTemplateLimits Payment = new ZnsTemplateLimits(200, 30, 30, 30);

        /// <summary>Template 618425 - thong bao duyet.</summary>
        public static readonly ZnsTemplateLimits Approve = new ZnsTemplateLimits(200, 30, 30, 200);

        /// <summary>Thong bao tu choi (Reject_Region la ly do nen de 200 ky tu).</summary>
        public static readonly ZnsTemplateLimits Reject = new ZnsTemplateLimits(200, 30, 30, 200);

        /// <summary>Thong bao hoan tat cat/bam seal.</summary>
        public static readonly ZnsTemplateLimits Complete = new ZnsTemplateLimits(200, 30, 30, 30);

        /// <summary>GIA DINH (placeholder) - gioi han template 618058 chua xac nhan.</summary>
        public static readonly ZnsTemplateLimits Cancel = new ZnsTemplateLimits(200, 30, 30, 200);

        private ZnsTemplateLimits(int customerName, int containerNo, int requestTs, int specificParam)
        {
            CustomerName = customerName;
            ContainerNo = containerNo;
            RequestTs = requestTs;
            SpecificParam = specificParam;
        }

        public int CustomerName { get; }

        public int ContainerNo { get; }

        public int RequestTs { get; }

        /// <summary>Gioi han cua tham so rieng cua tung template.</summary>
        public int SpecificParam { get; }
    }
}
