using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OK.Messaging.Api.Requests;
using OK.Messaging.Core.Managers;

namespace OK.Messaging.Api.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetMe()
        {
            return Ok(_userManager.GetUserById(CurrentUserId.Value));
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            bool isSuccess = _userManager.CreateUser(request.Username, request.Password, request.FullName);

            if (!isSuccess)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("block")]
        [Authorize]
        public IActionResult Block([FromBody] BlockUserRequest request)
        {
            bool isSuccess = _userManager.BlockUser(CurrentUserId.Value, request.Username);

            if (!isSuccess)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("unblock")]
        [Authorize]
        public IActionResult Unblock([FromBody] UnblockUserRequest request)
        {
            bool isSuccess = _userManager.UnblockUser(CurrentUserId.Value, request.Username);

            if (!isSuccess)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("activities")]
        [Authorize]
        public IActionResult GetActivities()
        {
            return Ok(_userManager.GetActivities(CurrentUserId.Value));
        }
    }
}