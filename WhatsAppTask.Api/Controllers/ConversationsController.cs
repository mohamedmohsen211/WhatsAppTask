using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DTO;

namespace WhatsAppTask.Api.Controllers
{
    [ApiController]
    [Route("api/conversations")]
    [Authorize]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationsController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpPost("by-phone")]
        public IActionResult GetOrCreateByPhone(CreateConversationByPhoneDto request)
        {
            var userId = GetUserId();

            var conversation = _conversationService.GetOrCreateByPhone(
                userId,
                request.PhoneNumber,
                request.Name,
                request.ImageUrl
            );

            return Ok(new
            {
                conversation.Id,
                PhoneNumber = conversation.Contact.PhoneNumber,
                ContactName = conversation.Contact.Name
            });
        }

        [HttpGet]
        public IActionResult GetMyConversations()
        {
            var userId = GetUserId();

            var conversations = _conversationService.GetUserConversations(userId)
                .Select(c => new ConversationResponseDto
                {
                    Id = c.Id,
                    ContactId = c.ContactId,
                    PhoneNumber = c.Contact.PhoneNumber,
                    ContactName = c.Contact.Name,
                    CreatedAt = c.CreatedAt
                });

            return Ok(conversations);
        }
    }
}
