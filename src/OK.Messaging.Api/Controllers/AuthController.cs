using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OK.Messaging.Api.Requests;
using OK.Messaging.Api.Responses;
using OK.Messaging.Core.Managers;
using System;

namespace OK.Messaging.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthManager _authManager;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, IAuthManager authManager)
        {
            _authManager = authManager;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            bool isValid = _authManager.Login(request.Username, request.Password, out int userId);

            if (!isValid)
            {
                return BadRequest("Username or password is incorrect.");
            }
            
            LoginResponse response = new LoginResponse();

            response.Token = CreateAuthToken(userId);
            response.ExpiresIn = DateTime.Now.AddMilliseconds(int.Parse(_configuration["Jwt:ExpiresInMs"]));

            return Ok(response);
        }

        #region Helpers

        private string CreateAuthToken(int userId)
        {
            string key = _configuration["Jwt:Key"];
            int expiresInMs = int.Parse(_configuration["Jwt:ExpiresInMs"]);

            return _authManager.CreateToken(userId, key, expiresInMs);
        }

        #endregion
    }
}