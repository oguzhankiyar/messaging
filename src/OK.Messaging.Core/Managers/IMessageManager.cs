using OK.Messaging.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OK.Messaging.Core.Managers
{
    public interface IMessageManager
    {
        List<MessageModel> GetMessages(int userId);

        Task<bool> CreateMessageAsync(int fromUserId, string toUsername, string content);
    }
}