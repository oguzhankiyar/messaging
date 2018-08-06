using NSubstitute;
using NUnit.Framework;
using OK.Messaging.Common.Entities;
using OK.Messaging.Common.Enumerations;
using OK.Messaging.Common.Models;
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
    public class UserManagerTests
    {
        private IUserRepository _userRepository;
        private IUserBlockRepository _userBlockRepository;
        private IActivityRepository _activityRepository;
        private IMapper _mapper;

        private IUserManager _userManager;

        [SetUp]
        public void SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _userBlockRepository = Substitute.For<IUserBlockRepository>();
            _activityRepository = Substitute.For<IActivityRepository>();
            _mapper = new AutoMapperImpl(ServiceCollectionExtensions.CreateMappingProfile());

            _userManager = new UserManager(_userRepository, _userBlockRepository, _activityRepository, _mapper);
        }

        [Test]
        public void GetUserById_ShouldReturnUser_WhenIdIsValid()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };

            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns(user);

            // Act
            UserModel userById = _userManager.GetUserById(1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(userById, Is.Not.Null);
                Assert.That(userById.Id, Is.EqualTo(user.Id));
            });
        }

        [Test]
        public void GetUserById_ShouldReturnNull_WhenIdIsInvalid()
        {
            // Arrange
            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns((UserEntity)null);

            // Act
            UserModel userById = _userManager.GetUserById(1);

            // Assert
            Assert.That(userById, Is.Null);
        }

        [Test]
        public void GetUserByUsername_ShouldReturnUser_WhenUsernameIsValid()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };

            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns(user);

            // Act
            UserModel userByUsername = _userManager.GetUserByUsername("mrkiyar");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(userByUsername, Is.Not.Null);
                Assert.That(userByUsername.Id, Is.EqualTo(user.Id));
            });
        }

        [Test]
        public void GetUserByUsername_ShouldReturnNull_WhenUsernameIsInvalid()
        {
            // Arrange
            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns((UserEntity)null);

            // Act
            UserModel userByUsername = _userManager.GetUserByUsername("mrkiyar");

            // Assert
            Assert.That(userByUsername, Is.Null);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void GetUserByUsername_ShouldThrowException_WhenUsernameIsNullOrEmpty(string username)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                UserModel userByUsername = _userManager.GetUserByUsername(username);
            });
        }

        [Test]
        public void LoginUser_ShouldReturnUser_WhenUsernameAndPasswordAreValid()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };

            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns(user);

            // Act
            UserModel loginUser = _userManager.LoginUser("mrkiyar", "123456");

            Assert.Multiple(() =>
            {
                Assert.That(loginUser, Is.Not.Null);
                Assert.That(loginUser.Id, Is.EqualTo(user.Id));
                Assert.That(loginUser.Username, Is.EqualTo(user.Username));
            });
        }

        [Test]
        public void LoginUser_ShouldReturnNull_WhenUsernameOrPasswordIsInvalid()
        {
            // Arrange
            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns((UserEntity)null);

            // Act
            UserModel loginUser = _userManager.LoginUser("mrkiyar", "123456");

            Assert.That(loginUser, Is.Null);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void LoginUser_ShouldThrowException_WhenUsernameIsNullOrEmpty(string username)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                UserModel loginUser = _userManager.LoginUser(username, "123456");
            });
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void LoginUser_ShouldThrowException_WhenPasswordIsNullOrEmpty(string password)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                UserModel loginUser = _userManager.LoginUser("mrkiyar", password);
            });
        }

        [Test]
        public void CreateUser_ShouldReturnTrue_WhenUserIsCreated()
        {
            // Arrange
            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns((UserEntity)null);

            _userRepository.Insert(Arg.Any<UserEntity>())
                           .Returns(new UserEntity() { Id = 1 });

            // Act
            bool isSuccess = _userManager.CreateUser("mrkiyar", "123456", "Oğuzhan Kiyar");

            // Assert
            Assert.That(isSuccess, Is.True);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void CreateUser_ShouldThrowException_WhenUsernameIsNullOrEmpty(string username)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                bool isSuccess = _userManager.CreateUser(username, "123456", "Oğuzhan Kiyar");
            });
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void CreateUser_ShouldThrowException_WhenPasswordIsNullOrEmpty(string password)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                bool isSuccess = _userManager.CreateUser("mrkiyar", password, "Oğuzhan Kiyar");
            });
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void CreateUser_ShouldThrowException_WhenFullNameIsNullOrEmpty(string fullName)
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                bool isSuccess = _userManager.CreateUser("mrkiyar", "123456", fullName);
            });
        }

        [Test]
        public void CreateUser_ShouldReturnFalse_WhenUsernameIsAlreadyTaken()
        {
            // Arrange
            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns(new UserEntity() { Id = 1, Username = "mrkiyar" });

            // Act
            bool isSuccess = _userManager.CreateUser("mrkiyar", "123456", "Oğuzhan Kiyar");

            // Assert
            Assert.That(isSuccess, Is.False);
        }

        [Test]
        public void IsUserBlocked_ShouldReturnTrue_WhenUserIsBlockedByOtherUser()
        {
            // Arrange
            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns(new UserBlockEntity());

            // Act
            bool isBlocked = _userManager.IsUserBlocked(1, 2);

            // Assert
            Assert.That(isBlocked, Is.True);
        }

        [Test]
        public void IsUserBlocked_ShouldReturnFalse_WhenUserIsNotBlockedByOtherUser()
        {
            // Arrange
            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns((UserBlockEntity)null);

            // Act
            bool isBlocked = _userManager.IsUserBlocked(1, 2);

            // Assert
            Assert.That(isBlocked, Is.False);
        }

        [Test]
        public void BlockUser_ShouldReturnTrue_WhenUserIsBlocked()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns((UserBlockEntity)null);

            _userBlockRepository.Insert(Arg.Any<UserBlockEntity>())
                                .Returns(new UserBlockEntity() { Id = 1 });

            _activityRepository.Insert(Arg.Any<ActivityEntity>())
                               .Returns(new ActivityEntity() { Id = 1 });

            // Act
            bool isBlocked = _userManager.BlockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.True);
        }

        [Test]
        public void BlockUser_ShouldReturnTrue_WhenUserIsBlockedBefore()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns(new UserBlockEntity());

            // Act
            bool isBlocked = _userManager.BlockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.True);
        }

        [Test]
        public void BlockUser_ShouldCallAddActivity_WhenUserIsBlocked()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns((UserBlockEntity)null);

            _userBlockRepository.Insert(Arg.Any<UserBlockEntity>())
                                .Returns(new UserBlockEntity() { Id = 1 });

            _activityRepository.Insert(Arg.Any<ActivityEntity>())
                               .Returns(new ActivityEntity() { Id = 1 });

            // Act
            bool isBlocked = _userManager.BlockUser(user.Id, blockedUser.Username);

            // Assert
            _activityRepository.Received(1)
                               .Insert(Arg.Any<ActivityEntity>());
        }

        [Test]
        public void BlockUser_ShouldReturnFalse_WhenUserIdIsInvalid()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return null;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            // Act
            bool isBlocked = _userManager.BlockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.False);
        }

        [Test]
        public void BlockUser_ShouldReturnFalse_WhenBlockedUsernameIsInvalid()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return null;

                               return null;
                           });

            // Act
            bool isBlocked = _userManager.BlockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.False);
        }

        [Test]
        public void BlockUser_ShouldReturnFalse_WhenUserIsNotBlocked()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns((UserBlockEntity)null);

            _userBlockRepository.Insert(Arg.Any<UserBlockEntity>())
                                .Returns(new UserBlockEntity() { Id = 0 });

            // Act
            bool isBlocked = _userManager.BlockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.False);
        }

        [Test]
        public void UnblockUser_ShouldReturnTrue_WhenUserIsUnblocked()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns(new UserBlockEntity());

            _userBlockRepository.Remove(Arg.Any<int>())
                                .Returns(true);

            _activityRepository.Insert(Arg.Any<ActivityEntity>())
                               .Returns(new ActivityEntity() { Id = 1 });

            // Act
            bool isBlocked = _userManager.UnblockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.True);
        }

        [Test]
        public void UnblockUser_ShouldReturnTrue_WhenUserIsNotBlockedBefore()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns((UserBlockEntity)null);

            // Act
            bool isBlocked = _userManager.UnblockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.True);
        }

        [Test]
        public void UnblockUser_ShouldCallAddActivity_WhenUserIsUnblocked()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns(new UserBlockEntity());

            _userBlockRepository.Remove(Arg.Any<int>())
                                .Returns(true);

            _activityRepository.Insert(Arg.Any<ActivityEntity>())
                               .Returns(new ActivityEntity() { Id = 1 });

            // Act
            bool isBlocked = _userManager.UnblockUser(user.Id, blockedUser.Username);

            // Assert
            _activityRepository.Received(1)
                               .Insert(Arg.Any<ActivityEntity>());
        }

        [Test]
        public void UnblockUser_ShouldReturnFalse_WhenUserIdIsInvalid()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return null;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            // Act
            bool isBlocked = _userManager.UnblockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.False);
        }

        [Test]
        public void UnblockUser_ShouldReturnFalse_WhenBlockedUsernameIsInvalid()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return null;

                               return null;
                           });

            // Act
            bool isBlocked = _userManager.UnblockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.False);
        }

        [Test]
        public void UnblockUser_ShouldReturnFalse_WhenUserIsNotUnblocked()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };
            UserEntity blockedUser = new UserEntity() { Id = 2, Username = "okiyar" };

            _userRepository.FindOne(i => true)
                           .ReturnsForAnyArgs(x =>
                           {
                               var func = ((Expression<Func<UserEntity, bool>>)x[0]).Compile();

                               if (func(user))
                                   return user;

                               if (func(blockedUser))
                                   return blockedUser;

                               return null;
                           });

            _userBlockRepository.FindOne(Arg.Any<Expression<Func<UserBlockEntity, bool>>>())
                                .Returns(new UserBlockEntity());

            _userBlockRepository.Remove(Arg.Any<int>())
                                .Returns(false);

            // Act
            bool isBlocked = _userManager.UnblockUser(user.Id, blockedUser.Username);

            // Assert
            Assert.That(isBlocked, Is.False);
        }

        [Test]
        public void GetActivities_ShouldReturnActivityList_WhenThereIsAnyActivity()
        {
            // Arrange
            var activities = new List<ActivityEntity>()
            {
                new ActivityEntity(),
                new ActivityEntity()
            };

            _activityRepository.FindMany(Arg.Any<Expression<Func<ActivityEntity, bool>>>())
                               .Returns(activities);

            // Act
            List<ActivityModel> results = _userManager.GetActivities(1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(activities.Count));
            });
        }

        [Test]
        public void GetActivities_ShouldReturnEmptyList_WhenThereIsNoActivity()
        {
            // Arrange
            var activities = new List<ActivityEntity>();

            _activityRepository.FindMany(Arg.Any<Expression<Func<ActivityEntity, bool>>>())
                               .Returns(activities);

            // Act
            List<ActivityModel> results = _userManager.GetActivities(1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results, Is.Not.Null);
                Assert.That(results.Count, Is.EqualTo(activities.Count));
            });
        }

        [Test]
        public void AddActivity_ShouldReturnTrue_WhenActivityIsCreated()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };

            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns(user);

            _activityRepository.Insert(Arg.Any<ActivityEntity>())
                               .Returns(new ActivityEntity() { Id = 1 });

            // Act
            bool isSuccess = _userManager.AddActivity(1, ActivityTypeEnum.SuccessLogin, "Some description.");

            // Assert
            Assert.That(isSuccess, Is.True);
        }

        [Test]
        public void AddActivity_ShouldReturnFalse_WhenUserIdIsInvalid()
        {
            // Arrange
            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns((UserEntity)null);

            // Act
            bool isSuccess = _userManager.AddActivity(1, ActivityTypeEnum.SuccessLogin, "Some description.");

            // Assert
            Assert.That(isSuccess, Is.False);
        }

        [Test]
        public void AddActivity_ShouldReturnFalse_WhenActivityIsNotCreated()
        {
            // Arrange
            UserEntity user = new UserEntity() { Id = 1, Username = "mrkiyar" };

            _userRepository.FindOne(Arg.Any<Expression<Func<UserEntity, bool>>>())
                           .Returns(user);

            _activityRepository.Insert(Arg.Any<ActivityEntity>())
                               .Returns(new ActivityEntity() { Id = 0 });
            // Act
            bool isSuccess = _userManager.AddActivity(1, ActivityTypeEnum.SuccessLogin, "Some description.");

            // Assert
            Assert.That(isSuccess, Is.False);
        }
    }
}