using ZnsNotificationRules.Models;

namespace ZnsNotificationRules.Contracts
{
    /// <summary>
    /// Tim so dien thoai nhan tin cho mot ho so. Nghiep vu tim so nam o adapter TOPO.
    /// Tra ve chuoi rong neu khong tim duoc, engine se ghi S kem ly do.
    /// </summary>
    public interface IZnsRecipientResolver
    {
        string ResolvePhone(ZnsTransactionNotifyDto item);
    }
}
