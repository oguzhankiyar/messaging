using OK.Messaging.Common.Entities;
using OK.Messaging.Common.Enumerations;
using OK.Messaging.Common.Models;
using OK.Messaging.Core.Managers;
using OK.Messaging.Core.Mapping;
using OK.Messaging.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OK.Messaging.Engine.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserBlockRepository _userBlockRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;

        public UserManager(IUserRepository userRepository, IUserBlockRepository userBlockRepository, IActivityRepository activityRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _userBlockRepository = userBlockRepository;
            _activityRepository = activityRepository;
            _mapper = mapper;
        }

        public UserModel GetUserById(int id)
        {
            UserEntity user = _userRepository.FindOne(x => x.Id == id);

            return _mapper.Map<UserEntity, UserModel>(user);
        }

        public UserModel GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException(nameof(username));
            }

            UserEntity user = _userRepository.FindOne(x => x.Username == username);

            return _mapper.Map<UserEntity, UserModel>(user);
        }

        public UserModel LoginUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException(nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(nameof(password));
            }

            string hashedPassword = HashPasswordWithMD5(password);

            UserEntity user = _userRepository.FindOne(x => x.Username == username && x.Password == hashedPassword);

            return _mapper.Map<UserEntity, UserModel>(user);
        }

        public bool CreateUser(string username, string password, string fullName)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException(nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException(nameof(password));
            }

            if (string.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException(nameof(password));
            }

            UserEntity userByUsername = _userRepository.FindOne(x => x.Username == username);

            if (userByUsername != null)
            {
                return false;
            }

            UserEntity user = new UserEntity();

            user.Username = username;
            user.Password = HashPasswordWithMD5(password);
            user.FullName = fullName;

            user = _userRepository.Insert(user);

            return user.Id > 0;
        }

        public bool IsUserBlocked(int userId, int blockedUserId)
        {
            UserBlockEntity userBlock = _userBlockRepository.FindOne(x => x.UserId == userId && x.BlockedUserId == blockedUserId);

            return userBlock != null;
        }

        public bool BlockUser(int userId, string blockedUsername)
        {
            UserEntity user = _userRepository.FindOne(x => x.Id == userId);

            if (user == null)
            {
                return false;
            }

            UserEntity blockedUser = _userRepository.FindOne(x => x.Username == blockedUsername);

            if (blockedUser == null)
            {
                return false;
            }

            UserBlockEntity userBlock = _userBlockRepository.FindOne(x => x.UserId == user.Id && x.BlockedUserId == blockedUser.Id);

            if (userBlock != null)
            {
                return true;
            }

            userBlock = new UserBlockEntity();

            userBlock.UserId = user.Id;
            userBlock.BlockedUserId = blockedUser.Id;

            userBlock = _userBlockRepository.Insert(userBlock);

            bool isBlocked = userBlock.Id > 0;

            if (isBlocked)
            {
                AddActivity(userId, ActivityTypeEnum.BlockUser, $"Blocked {blockedUser.Username}.");
            }

            return isBlocked;
        }

        public bool UnblockUser(int userId, string blockedUsername)
        {
            UserEntity user = _userRepository.FindOne(x => x.Id == userId);

            if (user == null)
            {
                return false;
            }

            UserEntity blockedUser = _userRepository.FindOne(x => x.Username == blockedUsername);

            if (blockedUser == null)
            {
                return false;
            }

            UserBlockEntity userBlock = _userBlockRepository.FindOne(x => x.UserId == user.Id && x.BlockedUserId == blockedUser.Id);

            if (userBlock == null)
            {
                return true;
            }

            bool isUnblocked = _userBlockRepository.Remove(userBlock.Id);

            if (isUnblocked)
            {
                AddActivity(userId, ActivityTypeEnum.UnblockUser, $"Unblocked {blockedUser.Username}.");
            }

            return isUnblocked;
        }

        public List<ActivityModel> GetActivities(int userId)
        {
            List<ActivityEntity> activities = _activityRepository.FindMany(x => x.UserId == userId).ToList();

            return _mapper.MapList<ActivityEntity, ActivityModel>(activities);
        }

        public bool AddActivity(int userId, ActivityTypeEnum activityType, string description)
        {
            UserEntity user = _userRepository.FindOne(x => x.Id == userId);

            if (user == null)
            {
                return false;
            }

            ActivityEntity activity = new ActivityEntity();

            activity.UserId = userId;
            activity.TypeId = activityType;
            activity.Description = description;

            activity = _activityRepository.Insert(activity);

            return activity.Id > 0;
        }

        #region Helpers

        private string HashPasswordWithMD5(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        #endregion
    }
}