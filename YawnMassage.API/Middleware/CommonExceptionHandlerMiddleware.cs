using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace YawnMassage.Api.Middleware
{
    public class CommonExceptionHandlerMiddleware
    {
        private RequestDelegate _next;
        private ILoggerService _loggerService;

        public CommonExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILoggerService loggerService)
        {
            _loggerService = loggerService;

            try
            {
                await _next(context);
            }
            catch (ConcurrencyException ex)
            {
                await HandleException(context, ex.Message, ex, HttpStatusCode.Conflict);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleException(context, ex.Message, ex, HttpStatusCode.Forbidden);
            }
            catch (AuthenticationException ex)
            {
                await HandleException(context, ex.Message, ex, HttpStatusCode.NotFound);
            }
            catch (YawnMassageException ex)
            {
                await HandleException(context, ex.Message, ex, HttpStatusCode.BadRequest);
            }
#if DEBUG
            catch (Exception ex)
            {
                await context.Response.WriteAsync(ex.ToString());
                throw;
            }
#endif
        }

        private async Task HandleException(HttpContext context, string displayError, Exception ex, HttpStatusCode statusCode)
        {
            _loggerService.LogError(ex, displayError);

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(displayError);
        }
    }
}
