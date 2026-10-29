using System;
using System.Collections.Generic;

namespace ZnsNotificationRules.Models
{
    /// <summary>
    /// Ho so dang cho gui, tra ve tu cong truy van.
    /// Truong dung chung va truong rieng cua 4 loai tin dang co san; loai tin moi dung Attributes, khong them property.
    /// </summary>
    public sealed class ZnsTransactionNotifyDto
    {
        public ZnsTransactionNotifyDto()
        {
            Attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public string KeyValue { get; set; }
        public string NotifyType { get; set; }
        public string ItemNo { get; set; }
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string ConsigneeTaxNo { get; set; }
        public string AuthorizedPhoneNo { get; set; }
        public string SealActionType { get; set; }
        public DateTime RequestTs { get; set; }
        public int FormNo { get; set; }

        // PAYMENT
        public DateTime TerOpConfirmedTs { get; set; }

        // APPROVE
        public string WorkerTeamName { get; set; }
        public string WorkerTeamContact { get; set; }

        // REJECT
        public string RejectReason { get; set; }

        // COMPLETE
        public DateTime CompleteTs { get; set; }

        /// <summary>Moc chan trung gui lai: PAYMENT/APPROVE/REJECT = moc nghiep vu, COMPLETE = CompleteTs.</summary>
        public DateTime SourceEventTs { get; set; }

        /// <summary>Thuoc tinh rieng cua loai tin (khoa trong ZnsAttributeKeys).</summary>
        public IDictionary<string, string> Attributes { get; }

        public string GetAttribute(string key)
        {
            return Attributes.TryGetValue(key, out var value) ? value : string.Empty;
        }
    }
}
