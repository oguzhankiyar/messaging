using Microsoft.Extensions.DependencyInjection;
using OK.Messaging.Common.Entities;
using OK.Messaging.Common.Models;
using OK.Messaging.Core.Handlers;
using OK.Messaging.Core.Logging;
using OK.Messaging.Core.Managers;
using OK.Messaging.Core.Mapping;
using OK.Messaging.Engine.Handlers;
using OK.Messaging.Engine.Logging;
using OK.Messaging.Engine.Managers;
using OK.Messaging.Engine.Mapping;

namespace OK.Messaging.Engine
{
    class MappingProfile : AutoMapper.Profile { }

    public static class ServiceCollectionExtensions
    {
        public static void AddEngineLayer(this IServiceCollection services)
        {
            services.AddSingleton<ILogger, NLoggerImpl>();

            services.AddTransient((provider) => { return CreateMappingProfile(); });
            services.AddTransient<IMapper, AutoMapperImpl>();

            services.AddTransient<IAuthManager, AuthManager>();
            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IMessageManager, MessageManager>();

            services.AddSingleton<IMessageStreamHandler, MessageStreamHandler>();
        }

        public static AutoMapper.Profile CreateMappingProfile()
        {
            var mappingProfile = new MappingProfile();

            mappingProfile.CreateMap<ActivityEntity, ActivityModel>();
            mappingProfile.CreateMap<ActivityModel, ActivityEntity>();

            mappingProfile.CreateMap<MessageEntity, MessageModel>();
            mappingProfile.CreateMap<MessageModel, MessageEntity>();

            mappingProfile.CreateMap<UserBlockEntity, UserBlockModel>();
            mappingProfile.CreateMap<UserBlockModel, UserBlockEntity>();

            mappingProfile.CreateMap<UserEntity, UserModel>();
            mappingProfile.CreateMap<UserModel, UserEntity>();

            return mappingProfile;
        }
    }
}