using Microsoft.Extensions.DependencyInjection;
using OK.Messaging.Core.Logging;
using OK.Messaging.Engine.Logging;

namespace OK.Messaging.Engine
{

    public static class ServiceCollectionExtensions
    {
        public static void AddEngineLayer(this IServiceCollection services)
        {
            services.AddSingleton<ILogger, NLoggerImpl>();
        }
    }
}