using OK.Messaging.Common.Models;
using System.Security.Claims;

namespace OK.Messaging.Core.Managers
{
    public interface IAuthManager
    {
        bool Login(string username, string password, out int userId);

        string CreateToken(int userId, string key, int expiresInMs);

        int? GetUserIdByPrincipal(ClaimsPrincipal principal);

        UserModel VerifyPrincipal(ClaimsPrincipal principal);
    }
}