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
        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var lists = _service.GetAll(userId)
                .Select(l => new
                {
                    l.Id,
                    l.Title,
                    l.Message,
                    l.CreatedAt
                });

            return Ok(lists);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var list = _service.GetById(userId, id);

            return Ok(new
            {
                list.Id,
                list.Title,
                list.Message,
                list.CreatedAt,
                Contacts = list.Items.Select(i => new
                {
                    i.Contact.Id,
                    i.Contact.PhoneNumber,
                    i.Contact.Name
                })
            });
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
