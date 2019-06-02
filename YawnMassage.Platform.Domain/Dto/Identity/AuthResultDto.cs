namespace YawnMassage.Platform.Domain.Dto.Identity
{
    using System.Collections.Generic;
    using YawnMassage.Platform.Domain.Dto.User;

    /// <summary>
    /// Defines the <see cref="AuthResultDto" />
    /// </summary>
    public class AuthResultDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether IsSucceeded
        /// </summary>
        public bool IsSucceeded { get; set; }

        /// <summary>
        /// Gets or sets the ErrorCode
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the User Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the Permissions
        /// </summary>
        public List<string> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the User
        /// </summary>
        public LoggedInUserDto User { get; set; }

       
    }

    public class MobileAuthResultDto:AuthResultDto
    {
        public UserDto UserDto { get; set; }
    }
}
