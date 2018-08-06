using Microsoft.AspNetCore.Http;
using OK.Messaging.Common.Models;
using OK.Messaging.Core.Logging;
using OK.Messaging.Core.Managers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OK.Messaging.Api.Middlewares
{
    public class RequestInformationMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public RequestInformationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthManager authManager)
        {
            UserModel authenticatedUser = authManager.VerifyPrincipal(context.User);
            
            _logger.SetThreadProperty("UserId", authenticatedUser?.Id);
            _logger.SetThreadProperty("RequestId", Guid.NewGuid().ToString());
            _logger.SetThreadProperty("IPAddress", context.Connection.RemoteIpAddress.MapToIPv4().ToString());
            _logger.SetThreadProperty("UserAgent", context.Request.Headers["User-Agent"].FirstOrDefault());

            await _next.Invoke(context);
        }
    }
}