using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsAppTask.BLL.Services;

namespace WhatsAppTask.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/message-lists")]
    public class MessageListsController : ControllerBase
    {
        private readonly MessageListService _service;

        public MessageListsController(MessageListService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create(CreateMessageListDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(_service.Create(userId, dto.Title, dto.Message));
        }

        [HttpPost("{id}/items")]
        public IActionResult AddItems(int id, AddListItemsDto dto)
        {
            _service.AddItems(id, dto.ContactIds);
            return NoContent();
        }
        [HttpPost("{id}/send")]
        public async Task<IActionResult> BulkSend(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _service.BulkSendAsync(userId, id);

            return Ok(new { message = "Messages sent successfully" });
        }
    }
}
