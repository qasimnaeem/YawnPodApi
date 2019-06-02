using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace YawnMassage.Api.Authorization
{
    public class CustomAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        private const string AnyGroupPrefix = "any:";

        public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(ClaimAuthorizeAttribute.POLICY_PREFIX))
            {
                var claim = policyName.Substring(ClaimAuthorizeAttribute.POLICY_PREFIX.Length);
                var policyBuilder = new AuthorizationPolicyBuilder();

                if (claim.StartsWith(AnyGroupPrefix))
                {
                    var claimMatch = claim.Substring(AnyGroupPrefix.Length);
                    //Group-insenstive match.
                    policyBuilder.RequireAssertion(context =>
                        context.User.HasClaim((c) => c.Type == CustomClaims.Permission && c.Value.EndsWith(claimMatch)));
                }
                else
                {
                    var requestContext = _httpContextAccessor.HttpContext.RequestServices.GetService<RequestContext>();

                    //Group-specific match.
                    var claimFullName = $"{requestContext.GroupId}:{claim}";
                    policyBuilder.RequireClaim(CustomClaims.Permission, claimFullName);
                }

                return Task.FromResult(policyBuilder.Build());
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        //public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    }
}
