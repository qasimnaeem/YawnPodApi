using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contexts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YawnMassage.Api.Middleware
{
    public class RequestContextResolverMiddleware
    {
        private RequestDelegate _next;

        public RequestContextResolverMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, RequestContext requestContext)
        {
            requestContext.GroupId = context.Request.Headers["X-GroupId"];
            if (string.IsNullOrEmpty(requestContext.GroupId))
                requestContext.GroupId = "*";
            
            string culture = context.Request.Headers["X-Culture"];
            requestContext.Culture = culture == null ? "*" : culture.ToUpper();

            requestContext.UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            requestContext.UserDisplayName = context.User.FindFirst(CustomClaims.UserDisplayName)?.Value;

            await _next(context);
        }
    }
}
