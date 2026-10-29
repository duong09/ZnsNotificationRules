using System;
using System.Globalization;
using System.Linq;

namespace ZnsNotificationRules.Formatting
{
    /// <summary>Dinh dang va cat chuoi dung chung cho cac tham so template.</summary>
    public static class ZnsText
    {
        public const string DateFormat = "dd/MM/yyyy HH:mm";

        private static readonly CultureInfo VietnameseCulture = CultureInfo.GetCultureInfo("vi-VN");

        /// <summary>Quy uoc TOPO: moc nam &lt;= 1900 la ngay rong.</summary>
        public static bool IsNullDateTime(DateTime value)
        {
            return value.Year <= 1900;
        }

        public static string FormatTs(DateTime value)
        {
            return value.ToString(DateFormat, VietnameseCulture);
        }

        public static string Truncate(string value, int maxLength)
        {
            var text = (value ?? string.Empty).Trim();

            return text.Length > maxLength ? text.Substring(0, maxLength) : text;
        }

        /// <summary>"Ten doi - Lien he". Thieu ve nao thi bo ve do, khong de dau "-" treo.</summary>
        public static string BuildWorkerTeamText(string workerTeamName, string workerTeamContact)
        {
            var parts = new[]
            {
                (workerTeamName ?? string.Empty).Trim(),
                (workerTeamContact ?? string.Empty).Trim()
            }.Where(part => part.Length > 0);

            return string.Join(" - ", parts);
        }
    }
}
