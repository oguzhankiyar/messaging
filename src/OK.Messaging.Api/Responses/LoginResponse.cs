using System;

namespace OK.Messaging.Api.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public DateTime ExpiresIn { get; set; }
    }
}