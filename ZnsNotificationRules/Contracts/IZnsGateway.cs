using ZnsNotificationRules.Models;

namespace ZnsNotificationRules.Contracts
{
    /// <summary>
    /// Cong gui tin sang Zalo (ZNS). Adapter phia TOPO implement, goi API va chon template id.
    /// Loi nghiep vu phia Zalo tra ve ZnsSendResult.Failure, khong nem exception.
    /// </summary>
    public interface IZnsGateway
    {
        ZnsSendResult Send(ZnsOutboundMessage message);
    }
}
