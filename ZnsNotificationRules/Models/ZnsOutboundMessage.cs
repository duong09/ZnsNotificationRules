using System.Collections.Generic;

namespace ZnsNotificationRules.Models
{
    /// <summary>Noi dung gui sang Zalo. Adapter cong gui tu chon template id theo NotifyType.</summary>
    public sealed class ZnsOutboundMessage
    {
        public ZnsOutboundMessage(string notifyType, string phone, IDictionary<string, string> templateData)
        {
            NotifyType = notifyType;
            Phone = phone;
            TemplateData = templateData;
        }

        public string NotifyType { get; }
        public string Phone { get; }
        public IDictionary<string, string> TemplateData { get; }
    }
}
