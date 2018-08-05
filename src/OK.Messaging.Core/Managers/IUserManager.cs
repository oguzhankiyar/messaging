using OK.Messaging.Common.Enumerations;
using OK.Messaging.Common.Models;
using System.Collections.Generic;

namespace OK.Messaging.Core.Managers
{
    public interface IUserManager
    {
        UserModel GetUserById(int id);

        UserModel GetUserByUsername(string username);

        UserModel LoginUser(string username, string password);

        UserModel CreateUser(string username, string password, string fullName);

        bool IsUserBlocked(int userId, int blockedUserId);

        bool BlockUser(int userId, string blockedUsername);
        
        bool UnblockUser(int userId, string blockedUsername);

        List<ActivityModel> GetActivities(int userId);

        bool AddActivity(int userId, ActivityTypeEnum activityType, string description);
    }
}