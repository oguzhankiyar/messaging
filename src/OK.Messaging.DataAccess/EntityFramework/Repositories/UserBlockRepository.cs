using OK.Messaging.Common.Entities;
using OK.Messaging.Core.Repositories;
using OK.Messaging.DataAccess.EntityFramework.DataContexts;

namespace OK.Messaging.DataAccess.EntityFramework.Repositories
{
    public class UserBlockRepository : BaseRepository<UserBlockEntity>, IUserBlockRepository
    {
        public UserBlockRepository(MessagingDataContext dataContext) : base(dataContext)
        {
        }
    }
}