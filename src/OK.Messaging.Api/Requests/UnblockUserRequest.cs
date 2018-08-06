using System.ComponentModel.DataAnnotations;

namespace OK.Messaging.Api.Requests
{
    public class UnblockUserRequest
    {
        [Required]
        public string Username { get; set; }
    }
}