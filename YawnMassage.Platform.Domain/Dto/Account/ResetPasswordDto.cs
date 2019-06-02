namespace YawnMassage.Platform.Domain.Dto.Account
{
    /// <summary>
    /// Defines the <see cref="ResetPasswordDto" />
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// Gets or sets the UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ConfirmPassword
        /// </summary>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the PasswordResetToken
        /// </summary>
        public string PasswordResetToken { get; set; }
    }
}
