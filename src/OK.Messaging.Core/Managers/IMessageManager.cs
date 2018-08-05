using OK.Messaging.Common.Models;
using System.Collections.Generic;

namespace OK.Messaging.Core.Managers
{
    public interface IMessageManager
    {
        List<MessageModel> GetMessages(int userId);

        bool CreateMessage(int fromUserId, string toUsername, string content);
    }
}