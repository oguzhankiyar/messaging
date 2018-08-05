using OK.Messaging.Common.Entities;
using OK.Messaging.Common.Models;
using OK.Messaging.Core.Managers;
using OK.Messaging.Core.Mapping;
using OK.Messaging.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace OK.Messaging.Engine.Managers
{
    public class MessageManager : IMessageManager
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public MessageManager(IMessageRepository messageRepository, IUserManager userManager, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public List<MessageModel> GetMessages(int userId)
        {
            List<MessageEntity> messages = _messageRepository.FindMany(x => x.FromUserId == userId || x.ToUserId == userId)
                                                             .OrderByDescending(x => x.CreatedDate)
                                                             .ToList();

            return _mapper.MapList<MessageEntity, MessageModel>(messages);
        }

        public bool CreateMessage(int fromUserId, string toUsername, string content)
        {
            UserModel fromUser = _userManager.GetUserById(fromUserId);

            if (fromUser == null)
            {
                return false;
            }

            UserModel toUser = _userManager.GetUserByUsername(toUsername);

            if (toUser == null)
            {
                return false;
            }

            bool isBlocked = _userManager.IsUserBlocked(toUser.Id, fromUser.Id);

            if (isBlocked)
            {
                return false;
            }

            MessageEntity message = new MessageEntity();

            message.FromUserId = fromUser.Id;
            message.ToUserId = toUser.Id;
            message.Content = content;

            message = _messageRepository.Insert(message);

            return message.Id > 0;
        }
    }
}