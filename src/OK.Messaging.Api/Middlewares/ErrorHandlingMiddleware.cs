using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using OK.Messaging.Core.Logging;
using System;
using System.Threading.Tasks;

namespace OK.Messaging.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(ILogger logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                string method = context.Request.Method;
                string url = context.Request.GetDisplayUrl();

                _logger.Error("ErrorHandlingMiddleware/RequestFailed", $"{method} {url}", ex);

                if (context.Items.TryGetValue("ElapsedTime", out object elapsedTime))
                {
                    _logger.DebugData("ErrorHandlingMiddleware/RequestCompleted", $"{method} {url}", new
                    {
                        Code = context.Response.StatusCode,
                        ElapsedTime = (long)elapsedTime
                    });
                }

                throw ex;
            }
        }
    }
}