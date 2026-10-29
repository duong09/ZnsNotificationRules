using System.Collections.Generic;
using ZnsNotificationRules.Contracts;
using ZnsNotificationRules.Rules;

namespace ZnsNotificationRules.Factory
{
    /// <summary>
    /// Diem dang ky duy nhat cua cac loai tin. Them template moi = them 1 dong o day.
    /// Tra ve instance moi moi lan goi (rule giu trang thai tu LoadConfig).
    /// </summary>
    public static class ZnsRuleCatalog
    {
        public static IReadOnlyList<IZnsNotificationRule> Create()
        {
            return new IZnsNotificationRule[]
            {
                new PaymentZnsRule(),
                new ApproveZnsRule(),
                new RejectZnsRule(),
                new CompleteZnsRule(),
                new CancelZnsRule()
            };
        }
    }
}
