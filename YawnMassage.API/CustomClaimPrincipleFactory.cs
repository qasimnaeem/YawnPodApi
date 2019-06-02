using YawnMassage.Platform.Domain.Contracts;
using YawnMassage.Platform.Domain.Documents;
using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Identity.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YawnMassage.Api
{
    public class CustomUserClaimsPrincipleFactory : UserClaimsPrincipalFactory<User, DocumentDbIdentityRole>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserPermissionService _userPermissionService;

        public CustomUserClaimsPrincipleFactory(
           UserManager<User> userManager,
           RoleManager<DocumentDbIdentityRole> roleManager,
           IOptions<IdentityOptions> optionsAccessor,
           IHttpContextAccessor httpContextAccessor,
           IUserPermissionService userPermissionService)
           : base(userManager, roleManager, optionsAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userPermissionService = userPermissionService;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var effectivePermissions = await _userPermissionService.GetEffectivePermissionsForUserAsync(user);
            var permissionClaims = effectivePermissions.Select(p => new Claim(CustomClaims.Permission, p));
            identity.AddClaims(permissionClaims);
            
            identity.AddClaim(new Claim(CustomClaims.UserDisplayName, $"{user.FullName}"));

            //This will be used to set permissions in AuthResult Dto
            _httpContextAccessor.HttpContext.Items["permissions"] = effectivePermissions;

            return identity;
        }
    }
}
