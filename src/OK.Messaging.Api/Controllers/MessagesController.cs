using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OK.Messaging.Api.Requests;
using OK.Messaging.Core.Managers;
using System.Threading.Tasks;

namespace OK.Messaging.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IMessageManager _messageManager;

        public MessagesController(IMessageManager messageManager)
        {
            _messageManager = messageManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_messageManager.GetMessages(CurrentUserId.Value));
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SendMessageRequest request)
        {
            bool isSent = await _messageManager.CreateMessageAsync(CurrentUserId.Value, request.Username, request.Content);

            if (!isSent)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}