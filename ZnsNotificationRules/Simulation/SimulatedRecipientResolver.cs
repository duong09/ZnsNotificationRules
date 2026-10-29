using ZnsNotificationRules.Contracts;
using ZnsNotificationRules.Models;

namespace ZnsNotificationRules.Simulation
{
    /// <summary>GIA LAP tim so nhan tin: lay thang AuthorizedPhoneNo cua ho so.</summary>
    public sealed class SimulatedRecipientResolver : IZnsRecipientResolver
    {
        public string ResolvePhone(ZnsTransactionNotifyDto item)
        {
            return item.AuthorizedPhoneNo ?? string.Empty;
        }
    }
}
