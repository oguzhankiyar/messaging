using Newtonsoft.Json;
using OK.Messaging.Common.Entities;
using OK.Messaging.Common.Models;
using OK.Messaging.Core.Handlers;
using OK.Messaging.Core.Managers;
using OK.Messaging.Core.Mapping;
using OK.Messaging.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OK.Messaging.Engine.Managers
{
    public class MessageManager : IMessageManager
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageStreamHandler _messageStreamHandler;
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public MessageManager(IMessageRepository messageRepository, IMessageStreamHandler messageStreamHandler, IUserManager userManager, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _messageStreamHandler = messageStreamHandler;
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

        public async Task<bool> CreateMessageAsync(int fromUserId, string toUsername, string content)
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

            if (string.IsNullOrEmpty(content))
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

            bool isSuccess = message.Id > 0;

            if (isSuccess)
            {
                object messageObject = new
                {
                    Sender = new
                    {
                        fromUser.Username,
                        fromUser.FullName
                    },
                    Content = content,
                    SentAt = DateTime.Now
                };

                await _messageStreamHandler.SendAsync(toUser.Username, JsonConvert.SerializeObject(messageObject));
            }

            return isSuccess;
        }
    }
}