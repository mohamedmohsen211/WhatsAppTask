using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WhatsAppTask.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/admins")]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminsController(IAdminService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create(CreateAdminDto dto)
        {
            return Ok(_service.Create(dto.Email, dto.Password));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateAdminDto dto)
        {
            return Ok(_service.Update(id, dto.Email));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }
    }
}
