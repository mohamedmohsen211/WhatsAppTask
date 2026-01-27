using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsAppTask.BLL.Interfaces;

namespace WhatsAppTask.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/auto-replies")]
    public class AutoRepliesController : ControllerBase
    {
        private readonly IAutoReplyService _service;

        public AutoRepliesController(IAutoReplyService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create(CreateAutoReplyDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(_service.Create(userId, dto.Keyword, dto.Reply));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(_service.GetAll(userId));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _service.Delete(userId, id);
            return NoContent();
        }
    }
}
