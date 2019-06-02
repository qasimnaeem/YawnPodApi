using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.Platform.Domain.Dto.Account
{
    /// <summary>
    /// Defines the <see cref="EmailConfirmationResult" />
    /// </summary>
    public class EmailConfirmationResult
    {
        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the OperationResult
        /// </summary>
        public OperationResult OperationResult { get; set; }

        /// <summary>
        /// Gets or sets the PasswordResetToken
        /// </summary>
        public string PasswordResetToken { get; set; }
    }
}
