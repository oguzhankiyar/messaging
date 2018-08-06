using NSubstitute;
using NUnit.Framework;
using OK.Messaging.Common.Entities;
using OK.Messaging.Common.Models;
using OK.Messaging.Core.Handlers;
using OK.Messaging.Core.Managers;
using OK.Messaging.Core.Mapping;
using OK.Messaging.Core.Repositories;
using OK.Messaging.Engine.Managers;
using OK.Messaging.Engine.Mapping;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace OK.Messaging.Engine.Tests.ManagerTests
{
    [TestFixture]
    public class MessageManagerTests
    {
        private IMessageRepository _messageRepository;
        private IMessageStreamHandler _messageStreamHandler;
        private IUserManager _userManager;
        private IMapper _mapper;
                
        private IMessageManager _messageManager;

        [SetUp]
        public void SetUp()
        {
            _messageRepository = Substitute.For<IMessageRepository>();
            _messageStreamHandler = Substitute.For<IMessageStreamHandler>();
            _userManager = Substitute.For<IUserManager>();
            _mapper = new AutoMapperImpl(ServiceCollectionExtensions.CreateMappingProfile());

            _messageManager = new MessageManager(_messageRepository, _messageStreamHandler, _userManager, _mapper);
        }

        [Test]
        public void GetMessages_ShouldReturnUserMessageList_WhenThereAreMessages()
        {
            // Arrange
            var messages = new List<MessageEntity>()
            {
                new MessageEntity(),
                new MessageEntity()
            };

            _messageRepository.FindMany(Arg.Any<Expression<Func<MessageEntity, bool>>>())
                              .Returns(messages);

            // Act
            List<MessageModel> results = _messageManager.GetMessages(1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(messages.Count));
            });
        }

        [Test]
        public void GetMessages_ShouldReturnEmptyList_WhenThereAreNoMessages()
        {
            // Arrange
            var messages = new List<MessageEntity>();

            _messageRepository.FindMany(Arg.Any<Expression<Func<MessageEntity, bool>>>())
                              .Returns(messages);

            // Act
            List<MessageModel> results = _messageManager.GetMessages(1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(messages.Count));
            });
        }

        [Test]
        public void CreateMessageAsync_ShouldReturnTrue_WhenParametersAreValid()
        {
            // Arrange
            _userManager.GetUserById(Arg.Any<int>())
                        .Returns(new UserModel() { Id = 1, Username = "mrkiyar" });

            _userManager.GetUserByUsername(Arg.Any<string>())
                        .Returns(new UserModel() { Id = 2, Username = "okiyar" });

            _userManager.IsUserBlocked(Arg.Any<int>(), Arg.Any<int>())
                        .Returns(false);

            _messageRepository.Insert(Arg.Any<MessageEntity>())
                              .Returns(new MessageEntity() { Id = 1 });

            // Act
            bool isSuccess = _messageManager.CreateMessageAsync(1, "okiyar", "Hello!").Result;

            // Assert
            Assert.That(isSuccess, Is.True);
        }

        [Test]
        public void CreateMessageAsync_ShouldReturnFalse_WhenFromUserIsInvalid()
        {
            // Arrange
            _userManager.GetUserById(Arg.Any<int>())
                        .Returns((UserModel)null);

            // Act
            bool isSuccess = _messageManager.CreateMessageAsync(1, "okiyar", "Hello!").Result;

            // Assert
            Assert.That(isSuccess, Is.False);
        }

        [Test]
        public void CreateMessageAsync_ShouldReturnFalse_WhenToUserIsInvalid()
        {
            // Arrange
            _userManager.GetUserById(Arg.Any<int>())
                        .Returns(new UserModel() { Id = 1, Username = "mrkiyar" });

            _userManager.GetUserByUsername(Arg.Any<string>())
                        .Returns((UserModel)null);

            // Act
            bool isSuccess = _messageManager.CreateMessageAsync(1, "okiyar", "Hello!").Result;

            // Assert
            Assert.That(isSuccess, Is.False);
        }

        [Test]
        public void CreateMessageAsync_ShouldReturnFalse_WhenContentIsNullOrEmpty()
        {
            // Arrange
            _userManager.GetUserById(Arg.Any<int>())
                        .Returns(new UserModel() { Id = 1, Username = "mrkiyar" });

            _userManager.GetUserByUsername(Arg.Any<string>())
                        .Returns(new UserModel() { Id = 2, Username = "okiyar" });

            // Act
            bool isSuccess = _messageManager.CreateMessageAsync(1, "okiyar", null).Result;

            // Assert
            Assert.That(isSuccess, Is.False);
        }

        [Test]
        public void CreateMessageAsync_ShouldReturnFalse_WhenToUserIsBlockedByFromUser()
        {
            // Arrange
            _userManager.GetUserById(Arg.Any<int>())
                        .Returns(new UserModel() { Id = 1, Username = "mrkiyar" });

            _userManager.GetUserByUsername(Arg.Any<string>())
                        .Returns(new UserModel() { Id = 2, Username = "okiyar" });

            _userManager.IsUserBlocked(Arg.Any<int>(), Arg.Any<int>())
                        .Returns(true);

            // Act
            bool isSuccess = _messageManager.CreateMessageAsync(1, "okiyar", "Hello!").Result;

            // Assert
            Assert.That(isSuccess, Is.False);
        }

        [Test]
        public void CreateMessageAsync_ShouldCallStreamSendAsync_WhenMessageIsCreated()
        {
            // Arrange
            _userManager.GetUserById(Arg.Any<int>())
                        .Returns(new UserModel() { Id = 1, Username = "mrkiyar" });

            _userManager.GetUserByUsername(Arg.Any<string>())
                        .Returns(new UserModel() { Id = 2, Username = "okiyar" });

            _userManager.IsUserBlocked(Arg.Any<int>(), Arg.Any<int>())
                        .Returns(false);

            _messageRepository.Insert(Arg.Any<MessageEntity>())
                              .Returns(new MessageEntity() { Id = 1 });

            // Act
            bool isSuccess = _messageManager.CreateMessageAsync(1, "okiyar", "Hello!").Result;

            // Assert
            _messageStreamHandler.Received(1)
                                 .SendAsync(Arg.Any<string>(), Arg.Any<string>())
                                 .Wait();
        }

        [Test]
        public void CreateMessageAsync_ShouldNotCallStreamSendAsync_WhenMessageIsNotCreated()
        {
            // Arrange
            _userManager.GetUserById(Arg.Any<int>())
                        .Returns(new UserModel() { Id = 1, Username = "mrkiyar" });

            _userManager.GetUserByUsername(Arg.Any<string>())
                        .Returns(new UserModel() { Id = 2, Username = "okiyar" });

            _userManager.IsUserBlocked(Arg.Any<int>(), Arg.Any<int>())
                        .Returns(false);

            _messageRepository.Insert(Arg.Any<MessageEntity>())
                              .Returns(new MessageEntity() { Id = 0 });

            // Act
            bool isSuccess = _messageManager.CreateMessageAsync(1, "okiyar", "Hello!").Result;

            // Assert
            _messageStreamHandler.DidNotReceive()
                                 .SendAsync(Arg.Any<string>(), Arg.Any<string>())
                                 .Wait();
        }
    }
}