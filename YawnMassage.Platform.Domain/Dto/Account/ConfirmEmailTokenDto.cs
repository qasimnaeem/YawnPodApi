namespace YawnMassage.Platform.Domain.Dto.Account
{
    /// <summary>
    /// Defines the <see cref="ConfirmEmailTokenDto" />
    /// </summary>
    public class ConfirmEmailTokenDto
    {
        /// <summary>
        /// Gets or sets the UserId
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the EmailConfirmationToken
        /// </summary>
        public string EmailConfirmationToken { get; set; }
    }
}
