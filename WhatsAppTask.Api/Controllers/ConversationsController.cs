using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DTO;
using WhatsAppTask.DTO.Enums;

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

            try
            {
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public IActionResult GetMyConversations()
        {
            var userId = GetUserId();

            var conversations = _conversationService
                .GetUserConversations(userId)
                .Select(c => new ConversationResponseDto
                {
                    Id = c.Id,
                    ContactId = c.ContactId,
                    PhoneNumber = c.Contact.PhoneNumber,
                    ContactName = c.Contact.Name,
                    ImageUrl = c.Contact.ImageUrl,
                    CreatedAt = c.CreatedAt,

                    LastMessage = c.Messages
                        .OrderByDescending(m => m.CreatedAt)
                        .Select(m => new LastMessageDto
                        {
                            Content = m.Content,
                            IsIncoming = m.IsIncoming,
                            Status = (MessageStatusDto)m.Status,
                            CreatedAt = m.CreatedAt
                        })
                        .FirstOrDefault()
                });

            return Ok(conversations);
        }

        [HttpDelete("{conversationId}")]
        public IActionResult DeleteConversation(int conversationId)
        {
            var userId = GetUserId();
            _conversationService.DeleteConversation(userId, conversationId);
            return NoContent();
        }

        [HttpPost("bulk-delete")]
        public IActionResult BulkDelete(BulkDeleteConversationsDto dto)
        {
            var userId = GetUserId();
            _conversationService.BulkDelete(userId, dto.ConversationIds);
            return NoContent();
        }


    }
}
