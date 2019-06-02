using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Dto.Account;
using YawnMassage.Platform.Domain.Dto.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YawnMassage.Platform.Domain.Dto.User;
using YawnMassage.Common.Domain.Dto;

namespace YawnMassage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IUserService _userServices;

        public AccountController(IIdentityService identityService,IUserService userService)
        {
            _identityService = identityService;
            _userServices = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<DocumentUpdateResultDto> Post([FromBody] UserDto user)
        {
            var result = await _userServices.CreateAsync(user);
            return result;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<AuthResultDto> Post([FromBody] LoginCredentialsDto loginCredentials)
        {
            var result = await _identityService.Authenticate(loginCredentials);
            return result;
        }

        [HttpPost("signout")]
        public async Task SignOut()
        {
            await _identityService.SignOutAsync();
        }

        [HttpPost("confirmuseremail")]
        public async Task<EmailConfirmationResult> ConfirmUserEmail([FromBody]ConfirmEmailTokenDto confirmEmailTokenDto)
        {
            return await _identityService.ActivateUserAccountAsync(confirmEmailTokenDto);
        }

        [HttpPost("resetpassword")]
        public async Task ResetPassword([FromBody]ResetPasswordDto resetPasswordDto)
        {
            await _identityService.ResetPasswordAsync(resetPasswordDto);
        }

        [HttpPost("forgotpassword")]
        public async Task ForgotPassword([FromBody]ForgotPasswordDto forgotPasswordDto)
        {
            await _identityService.SendPasswordResetEmailAync(forgotPasswordDto.Email);
        }


        #region # Mobile Contexts #

        [HttpGet("checkifaccountexist/{mobileNumber}")]
        public async Task<bool> CheckIfAccountExist(string mobileNumber)
        {
           return await _userServices.CheckIfAccountExistAsync(mobileNumber);
        }

        #endregion
    }
}
