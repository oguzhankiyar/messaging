using Microsoft.AspNetCore.Mvc;
using OK.Messaging.Core.Managers;

namespace OK.Messaging.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected int? CurrentUserId
        {
            get
            {
                IAuthManager authManager = (IAuthManager)Request.HttpContext.RequestServices.GetService(typeof(IAuthManager));

                return authManager.GetUserIdByPrincipal(Request.HttpContext.User);
            }
        }
    }
}