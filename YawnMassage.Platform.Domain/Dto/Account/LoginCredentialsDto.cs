namespace YawnMassage.Platform.Domain.Dto.Account
{
    /// <summary>
    /// Defines the <see cref="LoginCredentialsDto" />
    /// </summary>
    public class LoginCredentialsDto
    {
        /// <summary>
        /// Gets or sets the UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsRememberMe
        /// </summary>
        public bool IsRememberMe { get; set; }
    }
}
