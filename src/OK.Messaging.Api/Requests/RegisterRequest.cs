using System.ComponentModel.DataAnnotations;

namespace OK.Messaging.Api.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}