using OK.Messaging.Common.Models;
using System.Collections.Generic;

namespace OK.Messaging.Core.Managers
{
    public interface IMessageManager
    {
        List<MessageModel> GetMessages(int userId);

        MessageModel CreateMessage(int fromUserId, int toUserId, string content);
    }
}