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
        public IActionResult SendMessage(SendMessageRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                return BadRequest("Phone number is required");

            if (string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Message content is required");

            var userId = GetUserId();

            try
            {
                var message = _messageService.SendMessage(
                    userId,
                    request.PhoneNumber,
                    request.Content
                );

                return Ok(new MessageResponseDto
                {
                    Id = message.Id,
                    Content = message.Content,
                    IsIncoming = message.IsIncoming,
                    CreatedAt = message.CreatedAt
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("conversation/{conversationId}")]
        public IActionResult GetConversationMessages(int conversationId)
        {
            var userId = GetUserId();

            var messages = _messageService.GetConversationMessages(
                userId,
                conversationId
            );

            return Ok(messages.Select(m => new MessageResponseDto
            {
                Id = m.Id,
                Content = m.Content,
                IsIncoming = m.IsIncoming,
                CreatedAt = m.CreatedAt
            }));
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new Exception("User not authorized");

            return int.Parse(userIdClaim.Value);
        }
    }
}
