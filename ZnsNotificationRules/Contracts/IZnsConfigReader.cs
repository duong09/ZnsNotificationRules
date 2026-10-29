namespace ZnsNotificationRules.Contracts
{
    /// <summary>
    /// Doc gia tri so tu SYS_CODES (CODE_TP, DESCR/CODE_REF tuy cau hinh). Adapter phia TOPO implement.
    /// </summary>
    public interface IZnsConfigReader
    {
        int GetInt(string codeType, int defaultValue);
    }
}
