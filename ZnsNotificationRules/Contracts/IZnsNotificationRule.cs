using System.Collections.Generic;
using ZnsNotificationRules.Models;

namespace ZnsNotificationRules.Contracts
{
    /// <summary>
    /// Hop dong ma engine phu thuoc vao. Engine khong biet rule cu the nao dang chay (DIP).
    /// Them loai tin moi = them 1 class ke thua ZnsNotificationRuleBase, khong sua interface nay.
    /// </summary>
    public interface IZnsNotificationRule
    {
        /// <summary>NOTIFY_TYPE cua loai tin: PAYMENT, APPROVE, REJECT, COMPLETE, CANCEL.</summary>
        string NotifyType { get; }

        /// <summary>Ten tham so bat buoc phia Zalo (gom tham so chung va tham so rieng).</summary>
        IReadOnlyCollection<string> RequiredParams { get; }

        /// <summary>Doc cau hinh SYS_CODES ma rule can. Rule khong can cau hinh thi de trong.</summary>
        void LoadConfig(IZnsConfigReader config);

        /// <summary>Lay ho so dang cho gui cua loai tin nay (truy van va tong hop rieng cua rule).</summary>
        IReadOnlyList<ZnsTransactionNotifyDto> FindPending(ZnsRunContext context);

        /// <summary>Chuyen ho so dang cho thanh dong ZNS_TRANSACTION: N neu hop le, S kem ly do neu bo qua.</summary>
        IReadOnlyList<ZnsQueueEntry> BuildPendingEntries(ZnsRunContext context, IZnsRecipientResolver recipients);

        /// <summary>Ly do bo qua ho so, chuoi rong neu hop le.</summary>
        string GetSkipReason(ZnsTransactionNotifyDto item, string phone);

        /// <summary>Dung tui tham so gui Zalo, da cat theo gioi han cua template.</summary>
        IDictionary<string, string> BuildTemplateData(ZnsTransactionNotifyDto item);

        /// <summary>Cac tham so bat buoc dang thieu hoac rong trong tui.</summary>
        IReadOnlyList<string> GetMissingParams(IDictionary<string, string> templateData);
    }
}
