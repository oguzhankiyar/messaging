using NUnit.Framework;

namespace OK.Messaging.Engine.Tests.ManagerTests
{
    [TestFixture]
    public class UserManagerTests
    {
        public UserManagerTests()
        {

        }

        [Test]
        public void GetUserById_ShouldReturnUser_WhenIdIsValid()
        {

        }

        [Test]
        public void GetUserById_ShouldReturnNull_WhenIdIsInvalid()
        {

        }

        [Test]
        public void GetUserByUsername_ShouldReturnUser_WhenUsernameIsValid()
        {

        }

        [Test]
        public void GetUserByUsername_ShouldReturnNull_WhenUsernameIsInvalid()
        {

        }

        [Test]
        public void GetUserByUsername_ShouldThrowException_WhenUsernameIsNullOrEmpty()
        {

        }

        [Test]
        public void LoginUser_ShouldReturnUser_WhenUsernameAndPasswordAreValid()
        {

        }

        [Test]
        public void LoginUser_ShouldReturnUser_WhenUsernameOrPasswordIsInvalid()
        {

        }

        [Test]
        public void LoginUser_ShouldThrowException_WhenUsernameIsNullOrEmpty()
        {

        }

        [Test]
        public void LoginUser_ShouldThrowException_WhenPasswordIsNullOrEmpty()
        {

        }

        [Test]
        public void CreateUser_ShouldReturnTrue_WhenUserIsCreated()
        {

        }

        [Test]
        public void CreateUser_ShouldThrowException_WhenUsernameIsNullOrEmpty()
        {

        }

        [Test]
        public void CreateUser_ShouldThrowException_WhenPasswordIsNullOrEmpty()
        {

        }

        [Test]
        public void CreateUser_ShouldThrowException_WhenFullNameIsNullOrEmpty()
        {

        }

        [Test]
        public void CreateUser_ShouldReturnFalse_WhenUsernameIsAlreadyTaken()
        {

        }

        [Test]
        public void IsUserBlocked_ShouldReturnTrue_WhenUserIsBlockedByOtherUser()
        {

        }

        [Test]
        public void IsUserBlocked_ShouldReturnFalse_WhenUserIsNotBlockedByOtherUser()
        {

        }

        [Test]
        public void BlockUser_ShouldReturnTrue_WhenUserIsBlocked()
        {

        }

        [Test]
        public void BlockUser_ShouldReturnTrue_WhenUserIsBlockedBefore()
        {

        }

        [Test]
        public void BlockUser_ShouldCallAddActivity_WhenUserIsBlocked()
        {

        }

        [Test]
        public void BlockUser_ShouldReturnFalse_WhenUserIdIsInvalid()
        {

        }

        [Test]
        public void BlockUser_ShouldReturnFalse_WhenBlockedUsernameIsInvalid()
        {

        }

        [Test]
        public void BlockUser_ShouldReturnFalse_WhenUserIsNotBlocked()
        {

        }

        [Test]
        public void UnblockUser_ShouldReturnTrue_WhenUserIsUnblocked()
        {

        }

        [Test]
        public void UnblockUser_ShouldReturnTrue_WhenUserIsNotBlockedBefore()
        {

        }

        [Test]
        public void UnblockUser_ShouldCallAddActivity_WhenUserIsUnblocked()
        {

        }

        [Test]
        public void UnblockUser_ShouldReturnFalse_WhenUserIdIsInvalid()
        {

        }

        [Test]
        public void UnblockUser_ShouldReturnFalse_WhenBlockedUsernameIsInvalid()
        {

        }

        [Test]
        public void UnblockUser_ShouldReturnFalse_WhenUserIsNotUnblocked()
        {

        }

        [Test]
        public void GetActivities_ShouldReturnActivityList_WhenThereIsAnyActivity()
        {

        }

        [Test]
        public void GetActivities_ShouldReturnEmptyList_WhenThereIsNoActivity()
        {

        }

        [Test]
        public void AddActivity_ShouldReturnTrue_WhenActivityIsCreated()
        {

        }

        [Test]
        public void AddActivity_ShouldReturnFalse_WhenUserIdIsInvalid()
        {

        }

        [Test]
        public void AddActivity_ShouldReturnFalse_WhenActivityIsNotCreated()
        {

        }
    }
}