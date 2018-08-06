using System.ComponentModel.DataAnnotations;

namespace OK.Messaging.Api.Requests
{
    public class SendMessageRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Content { get; set; }
    }
}