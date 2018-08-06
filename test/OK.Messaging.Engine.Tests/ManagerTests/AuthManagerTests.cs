using NSubstitute;
using NUnit.Framework;
using OK.Messaging.Common.Enumerations;
using OK.Messaging.Common.Models;
using OK.Messaging.Core.Managers;
using OK.Messaging.Engine.Managers;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace OK.Messaging.Engine.Tests.ManagerTests
{
    [TestFixture]
    public class AuthManagerTests
    {
        private IUserManager _userManager;
        private IAuthManager _authManager;

        [SetUp]
        public void SetUp()
        {
            _userManager = Substitute.For<IUserManager>();

            _authManager = new AuthManager(_userManager);
        }

        [Test]
        public void Login_ShouldReturnTrue_WhenUsernameAndPasswordAreValid()
        {
            // Arrange
            UserModel user = new UserModel() { Id = 1, Username = "mrkiyar", FullName = "Oğuzhan Kiyar" };

            _userManager.LoginUser(Arg.Any<string>(), Arg.Any<string>())
                        .Returns(user);

            // Act
            bool isSuccess = _authManager.Login("mrkiyar", "123456", out int userId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(isSuccess, Is.True);
                Assert.That(userId, Is.EqualTo(user.Id));
            });
        }

        [Test]
        public void Login_ShouldCallAddActivity_WhenUsernameAndPasswordAreValid()
        {
            // Arrange
            UserModel user = new UserModel() { Id = 1, Username = "mrkiyar", FullName = "Oğuzhan Kiyar" };

            _userManager.LoginUser(Arg.Any<string>(), Arg.Any<string>())
                        .Returns(user);

            // Act
            bool isSuccess = _authManager.Login("mrkiyar", "123456", out int userId);

            // Assert
            _userManager.Received(1)
                        .AddActivity(user.Id, ActivityTypeEnum.SuccessLogin, Arg.Any<string>());
        }

        [Test]
        public void Login_ShouldReturnFalse_WhenUsernameOrPasswordIsInvalid()
        {
            // Arrange
            _userManager.LoginUser(Arg.Any<string>(), Arg.Any<string>())
                        .Returns((UserModel)null);

            // Act
            bool isSuccess = _authManager.Login("mrkiyar", "123456", out int userId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(isSuccess, Is.False);
                Assert.That(userId, Is.EqualTo(0));
            });
        }

        [Test]
        public void Login_ShouldCallAddActivity_WhenOnlyPasswordIsInvalid()
        {
            // Arrange
            UserModel user = new UserModel() { Id = 1, Username = "mrkiyar", FullName = "Oğuzhan Kiyar" };

            _userManager.LoginUser(Arg.Any<string>(), Arg.Any<string>())
                        .Returns((UserModel)null);

            _userManager.GetUserByUsername(Arg.Any<string>())
                        .Returns(user);

            // Act
            bool isSuccess = _authManager.Login("mrkiyar", "654321", out int userId);

            // Assert
            _userManager.Received(1)
                        .AddActivity(user.Id, ActivityTypeEnum.InvalidLogin, Arg.Any<string>());
        }

        [Test]
        public void GetUserIdByPrincipal_ShouldReturnCorrectId_WhenPrincipalIsValid()
        {
            // Arrange
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, "1")
            }.ToList();

            // Act
            int? userId = _authManager.GetUserIdByPrincipal(new ClaimsPrincipal(new ClaimsIdentity(claims)));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(userId, Is.Not.Null);
                Assert.That(userId, Is.EqualTo(1));
            });
        }

        [Test]
        public void VerifyPrincipal_ShouldReturnCorrectUser_WhenPrincipalIsValid()
        {
            // Arrange
            UserModel user = new UserModel() { Id = 1, Username = "mrkiyar", FullName = "Oğuzhan Kiyar" };

            _userManager.GetUserById(Arg.Any<int>())
                        .Returns(user);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            }.ToList();

            // Act
            UserModel verifiedUser = _authManager.VerifyPrincipal(new ClaimsPrincipal(new ClaimsIdentity(claims)));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(verifiedUser, Is.Not.Null);
                Assert.That(verifiedUser.Id, Is.EqualTo(user.Id));
            });
        }
    }
}