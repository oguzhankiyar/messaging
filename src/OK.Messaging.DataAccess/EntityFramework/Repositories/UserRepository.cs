using OK.Messaging.Common.Entities;
using OK.Messaging.Core.Repositories;
using OK.Messaging.DataAccess.EntityFramework.DataContexts;

namespace OK.Messaging.DataAccess.EntityFramework.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(MessagingDataContext dataContext) : base(dataContext)
        {
        }
    }
}