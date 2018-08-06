using NUnit.Framework;

namespace OK.Messaging.Engine.Tests.ManagerTests
{
    [TestFixture]
    public class MessageManagerTests
    {
        public MessageManagerTests()
        {

        }

        [Test]
        public void GetMessages_ShouldReturnUserMessageList_WhenThereAreMessages()
        {

        }

        [Test]
        public void GetMessages_ShouldReturnEmptyList_WhenThereAreNoMessages()
        {

        }

        [Test]
        public void CreateMessageAsync_ShouldReturnTrue_WhenParametersAreValid()
        {

        }

        [Test]
        public void CreateMessageAsync_ShouldReturnFalse_WhenFromUserIsInvalid()
        {

        }

        [Test]
        public void CreateMessageAsync_ShouldReturnFalse_WhenToUserIsInvalid()
        {

        }

        [Test]
        public void CreateMessageAsync_ShouldReturnFalse_WhenContentIsNullOrEmpty()
        {

        }

        [Test]
        public void CreateMessageAsync_ShouldReturnFalse_WhenToUserIsBlockedByFromUser()
        {

        }

        [Test]
        public void CreateMessageAsync_ShouldCallStreamSendAsync_WhenMessageIsCreated()
        {

        }

        [Test]
        public void CreateMessageAsync_ShouldNotCallStreamSendAsync_WhenMessageIsNotCreated()
        {

        }
    }
}