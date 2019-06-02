using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Platform.Domain.Dto.Account;
using YawnMassage.Platform.Domain.Dto.Identity;
using YawnMassage.Common.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace YawnMassage.Platform.Services
{
    public class UserIdentityPlatformService : IIdentityService
    {
        private readonly ISystemDataContext _systemDataContext;
        public UserIdentityPlatformService(ISystemDataContext systemDataContext)
        {
            _systemDataContext = systemDataContext;
        }

        public Task<EmailConfirmationResult> ActivateUserAccountAsync(ConfirmEmailTokenDto confirmEmailTokenDto)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResultDto> Authenticate(LoginCredentialsDto loginCredentials)
        {
            throw new NotImplementedException();
        }

        public async Task RefreshSigninForCurrentUserAsync()
        {
            await Task.CompletedTask;
        }

        public async Task CreateUserAsync(User user)
        {
            user.SecurityStamp = Guid.NewGuid().ToString("D");
            await _systemDataContext.CreateDocumentAsync(user);
        }

        public Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetEmailAync(string email)
        {
            throw new NotImplementedException();
        }

        public Task SendUserActivationEmailAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserTokenAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
