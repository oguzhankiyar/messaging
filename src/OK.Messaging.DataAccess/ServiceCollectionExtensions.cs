using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OK.Messaging.Core.Repositories;
using OK.Messaging.DataAccess.EntityFramework.DataContexts;
using OK.Messaging.DataAccess.EntityFramework.Repositories;

namespace OK.Messaging.DataAccess
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataAccessLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MessagingDataContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.BuildServiceProvider().GetService<MessagingDataContext>().Database.Migrate();

            services.AddTransient<IActivityRepository, ActivityRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IUserBlockRepository, UserBlockRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}