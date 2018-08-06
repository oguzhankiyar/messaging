using System.ComponentModel.DataAnnotations;

namespace OK.Messaging.Api.Requests
{
    public class BlockUserRequest
    {
        [Required]
        public string Username { get; set; }
    }
}