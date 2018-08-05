using OK.Messaging.Common.Entities;
using OK.Messaging.Core.Repositories;
using OK.Messaging.DataAccess.EntityFramework.DataContexts;

namespace OK.Messaging.DataAccess.EntityFramework.Repositories
{
    public class ActivityRepository : BaseRepository<ActivityEntity>, IActivityRepository
    {
        public ActivityRepository(MessagingDataContext dataContext) : base(dataContext)
        {
        }
    }
}