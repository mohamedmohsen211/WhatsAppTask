using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DTO;

namespace WhatsAppTask.Api.Controllers
{
    [ApiController]
    [Route("api/messages")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(SendMessageRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                return BadRequest("Phone number is required");

            if (string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Message content is required");

            if (request.Content.Length > 4096)
                return BadRequest("Message too long");

            var userId = GetUserId();

            var message = await _messageService.SendMessageAsync(
                userId,
                request.PhoneNumber,
                request.Content
            );

            return Ok(new
            {
                message.Id,
                message.Content,
                message.Status,
                message.CreatedAt,
                message.ConversationId
            });
        }

        [HttpGet("conversation/{conversationId}")]
        public IActionResult GetConversationMessages(int conversationId)
        {
            var userId = GetUserId();

            var messages = _messageService.GetConversationMessages(
                userId,
                conversationId
            );

            return Ok(messages.Select(m => new
            {
                m.Id,
                m.Content,
                m.IsIncoming,
                m.Status,
                m.CreatedAt
            }));
        }
        [HttpGet("conversations")]
        public IActionResult GetConversations()
        {
            var userId = GetUserId();
            return Ok(_messageService.GetConversationsWithLastMessage(userId));
        }
        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
                throw new UnauthorizedAccessException();

            return int.Parse(claim.Value);
        }
    }
}
