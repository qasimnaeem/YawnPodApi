using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Account;
using YawnMassage.Platform.Domain.Dto.Identity;
using YawnMassage.Platform.Domain.Dto.User;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto;
using YawnMassage.Common.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Owin.Security.DataProtection;
using SendGrid;
using IDataProtector = Microsoft.AspNetCore.DataProtection.IDataProtector;

namespace YawnMassage.Platform.Services
{
    [ExcludeFromCodeCoverage]
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfigurationReaderService _configurationReaderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RequestContext _requestContext;
        private readonly IUserContextService _webUserContextService;
        private readonly IAlertNotificationRequestService _alertNotificationRequestService;
        private readonly IUserPermissionService _userPermissionService;
        public IdentityService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfigurationReaderService configurationReaderService,
            IPermissionService permissionService,
            IHttpContextAccessor httpContextAccessor,
            RequestContext requestContext,
            IUserContextService webUserContextService,
            IAlertNotificationRequestService alertNotificationRequestService, IUserPermissionService userPermissionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configurationReaderService = configurationReaderService;
            _httpContextAccessor = httpContextAccessor;
            _requestContext = requestContext;
            _webUserContextService = webUserContextService;
            _alertNotificationRequestService = alertNotificationRequestService;
            _userPermissionService = userPermissionService;
        }

        public async Task<EmailConfirmationResult> ActivateUserAccountAsync(ConfirmEmailTokenDto confirmEmailTokenDto)
        {
            EmailConfirmationResult returnResult = new EmailConfirmationResult
            {
                OperationResult = new OperationResult()
            };
            try
            {


                var user = await _userManager.FindByIdAsync(confirmEmailTokenDto.UserId);

                if (user == null)
                    throw new YawnMassageException("ERROR_NO_USER_FOUND");

                //activate user
                var result = await _userManager.ConfirmEmailAsync(user, confirmEmailTokenDto.EmailConfirmationToken);

                if (result.Succeeded)
                {
                    //password reset token
                    var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string encodedPasswordResetToken = WebUtility.UrlEncode(passwordResetToken);
                    returnResult.PasswordResetToken = encodedPasswordResetToken;
                    returnResult.Email = user.Email;
                    returnResult.OperationResult.IsSuccess = true;
                }
                else
                {
                    returnResult.OperationResult.IsSuccess = false;
                    returnResult.OperationResult.Message = "ERROR_INVALID_ACTIVATION_LINK";
                }

            }
            catch (Exception ex)
            {
                returnResult.OperationResult.IsSuccess = false;
                returnResult.OperationResult.Message = ex.Message;
            }

            return returnResult;
        }

        public async Task<AuthResultDto> Authenticate(LoginCredentialsDto loginCredentials)
        {
            var result = new AuthResultDto();
            try
            {
                //Modifiy this logic accordingly based on the authentication technology used.
                var user = await _userManager.FindByNameAsync(loginCredentials.UserName);
                if (user != null && !user.IsDeleted)
                {
                    if (!user.IsEmailConfirmed)
                    {
                        result.IsSucceeded = false;
                        result.ErrorCode = "ERROR_USER_NOT_ACTIVATED";
                    }
                    else
                    {
                        var response = await _signInManager.PasswordSignInAsync(loginCredentials.UserName, loginCredentials.Password, loginCredentials.IsRememberMe,
                            lockoutOnFailure: false);
                        if (response.Succeeded)
                        {
                            var permissions = (List<string>)_httpContextAccessor.HttpContext.Items["permissions"];
                            var userDto = GetUserDto(user);
                            return new AuthResultDto { IsSucceeded = true, Permissions = permissions, User = userDto };
                        }
                        else if (response.RequiresTwoFactor)
                        {
                            return new AuthResultDto { IsSucceeded = true };
                        }
                    }
                }
                else
                {
                    result.IsSucceeded = false;
                    result.ErrorCode = "ERROR_INVALID_LOGIN_CREDENTIALS";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.IsSucceeded = true;
                result.ErrorCode = e.Message;

            }
            return result;
        }

        public async Task RefreshSigninForCurrentUserAsync()
        {
            if (string.IsNullOrEmpty(_requestContext.UserId))
                throw new Exception("Cannot refresh signin without a current user.");

            var user = await _userManager.FindByIdAsync(_requestContext.UserId);

            await _signInManager.RefreshSignInAsync(user);
            _webUserContextService.RefreshClientSessionData();
        }

        public async Task CreateUserAsync(User user)
        {
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                throw new YawnMassageException("ERROR_USER_CREATION");
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            if (!resetPasswordDto.Password.Equals(resetPasswordDto.ConfirmPassword))
                throw new YawnMassageException("ERROR_PASSWORD_MISMATCH");

            var user = await _userManager.FindByIdAsync(resetPasswordDto.UserId.ToString());

            if (user == null)
                throw new YawnMassageException("ERROR_NO_USER_FOUND");

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.PasswordResetToken, resetPasswordDto.Password);

            if (!result.Succeeded)
            {
                if (result.Errors.Any(e => e.Code == "InvalidToken"))
                    throw new YawnMassageException("ERROR_INVALID_TOKEN");

                throw new YawnMassageException("ERROR_PASSWORD_SET");
            }
        }

        public async Task SendPasswordResetEmailAync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null && user.IsEmailConfirmed)  // Don't reveal that the user does not exist or is not confirmed
            {
                //password reset token
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                string encodedPasswordResetToken = WebUtility.UrlEncode(passwordResetToken);

                var systemHostUrlGetTask = _configurationReaderService.GetValueAsync(SystemKeys.ConfigurationKeys.SystemHostUrl);
                var setPasswordActionGetTask = _configurationReaderService.GetValueAsync(SystemKeys.ConfigurationKeys.SetPasswordAction);

                var systemHostUrl = await systemHostUrlGetTask;
                var setPasswordControllerAction = await setPasswordActionGetTask;
                var passwordResetUrl = $"{systemHostUrl}/{setPasswordControllerAction}/{user.Id}/{user.Email}/{encodedPasswordResetToken}/{true}";

                await _alertNotificationRequestService.NotifyAsync(
                    SystemKeys.AlertTemplateKeys.ResetPassword,
                    SystemKeys.AlertChannelKeys.Email,
                    new Dictionary<string, object> { { AlertObjectContextTypes.User, user.Id } },
                    new Dictionary<string, string> { { "passwordResetUrl", passwordResetUrl } },
                    user.Id);
            }
        }

        public async Task SendUserActivationEmailAsync(User user)
        {
            var tkn = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string tknEncode = WebUtility.UrlEncode(tkn);

            var systemHostUrlTask = _configurationReaderService.GetValueAsync(SystemKeys.ConfigurationKeys.SystemHostUrl);
            var activationEmailControllerActionUrlTask = _configurationReaderService.GetValueAsync(SystemKeys.ConfigurationKeys.ActivationEmailAction);

            var systemHostUrl = await systemHostUrlTask;
            var activationEmailControllerAction = await activationEmailControllerActionUrlTask;
            var activationUrl = $"{systemHostUrl}/{activationEmailControllerAction}/{user.Id}/{tknEncode}";

            await _alertNotificationRequestService.NotifyAsync(
                SystemKeys.AlertTemplateKeys.UserActivation,
                SystemKeys.AlertChannelKeys.Email,
                new Dictionary<string, object> { { AlertObjectContextTypes.User, user.Id } },
                new Dictionary<string, string> { { "activationUrl", activationUrl } },
                user.Id);

            user.IsActivationEmailSent = true;

            await _userManager.UpdateAsync(user);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GetUserTokenAsync(User user)
        {
            return await _userManager.GenerateUserTokenAsync(user,TokenOptions.DefaultProvider,"*");
        }

        private async Task<bool> IsUserActivatedAsync(User user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        private LoggedInUserDto GetUserDto(User user)
        {
            var dto = new LoggedInUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Culture = user.Culture,
                TimeZone = user.TimeZone,
                Email = user.Email
            };
            return dto;
        }

    }
}
