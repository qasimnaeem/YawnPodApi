namespace YawnMassage.Common.Domain.Dto
{
    /// <summary>
    /// Defines the <see cref="OperationResult" />
    /// </summary>
    public class OperationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether IsSuccess
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the Message
        /// </summary>
        public string Message { get; set; }
    }
}
