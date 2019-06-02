using System.Threading.Tasks;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Account;
using YawnMassage.Platform.Domain.Dto.Identity;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IIdentityService" />
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// The ActivateUserAccountAsync
        /// </summary>
        /// <param name="confirmEmailTokenDto">The confirmEmailTokenDto<see cref="ConfirmEmailTokenDto"/></param>
        /// <returns>The <see cref="Task{EmailConfirmationResult}"/></returns>
        Task<EmailConfirmationResult> ActivateUserAccountAsync(ConfirmEmailTokenDto confirmEmailTokenDto);

        /// <summary>
        /// The Authenticate
        /// </summary>
        /// <param name="loginCredentials">The loginCredentials<see cref="LoginCredentialsDto"/></param>
        /// <returns>The <see cref="Task{AuthResultDto}"/></returns>
        Task<AuthResultDto> Authenticate(LoginCredentialsDto loginCredentials);

        /// <summary>
        /// The RefreshSigninForCurrentUserAsync
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        Task RefreshSigninForCurrentUserAsync();

        /// <summary>
        /// The CreateUserAsync
        /// </summary>
        /// <param name="userDto">The userDto<see cref="User"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task CreateUserAsync(User userDto);

        /// <summary>
        /// The ResetPasswordAsync
        /// </summary>
        /// <param name="resetPasswordDto">The resetPasswordDto<see cref="ResetPasswordDto"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        /// <summary>
        /// The SendPasswordResetEmailAync
        /// </summary>
        /// <param name="email">The email<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task SendPasswordResetEmailAync(string email);

        /// <summary>
        /// The SendUserActivationEmailAsync
        /// </summary>
        /// <param name="user">The user<see cref="User"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task SendUserActivationEmailAsync(User user);

        /// <summary>
        /// The SignOutAsync
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        Task SignOutAsync();

        Task<string> GetUserTokenAsync(User user);
    }
}
