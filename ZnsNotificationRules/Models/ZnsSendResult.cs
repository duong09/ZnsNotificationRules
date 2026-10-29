namespace ZnsNotificationRules.Models
{
    /// <summary>Ket qua gui mot tin. Cong gui tra ve ket qua thay vi nem exception khi Zalo tu choi.</summary>
    public sealed class ZnsSendResult
    {
        private ZnsSendResult(bool isSuccess, string messageId, string errorMessage)
        {
            IsSuccess = isSuccess;
            MessageId = messageId;
            ErrorMessage = errorMessage;
        }

        public static ZnsSendResult Success(string messageId)
        {
            return new ZnsSendResult(true, messageId ?? string.Empty, string.Empty);
        }

        public static ZnsSendResult Failure(string errorMessage)
        {
            return new ZnsSendResult(false, string.Empty, errorMessage ?? string.Empty);
        }

        public bool IsSuccess { get; }
        public string MessageId { get; }
        public string ErrorMessage { get; }
    }
}
