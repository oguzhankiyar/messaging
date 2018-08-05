using OK.Messaging.Common.Entities;
using OK.Messaging.Core.Repositories;
using OK.Messaging.DataAccess.EntityFramework.DataContexts;

namespace OK.Messaging.DataAccess.EntityFramework.Repositories
{
    public class MessageRepository : BaseRepository<MessageEntity>, IMessageRepository
    {
        public MessageRepository(MessagingDataContext dataContext) : base(dataContext)
        {
        }
    }
}