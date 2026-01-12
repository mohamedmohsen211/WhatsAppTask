using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DTO;

namespace WhatsAppTask.Api.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        [HttpPost]
        public IActionResult Create(CreateContactRequestDto request)
        {
            var userId = GetUserId();

            var contact = _contactService.CreateContact(
                userId,
                request.PhoneNumber,
                request.Name,
                request.ImageUrl
            );
            if (contact == null)
                return BadRequest("Contact already exists");

            return Ok(new ContactResponseDto
            {
                Id = contact.Id,
                PhoneNumber = contact.PhoneNumber,
                Name = contact.Name,
                ImageUrl = contact.ImageUrl,
                CreatedAt = contact.CreatedAt
            });
        }

        [HttpGet]
        public IActionResult GetMyContacts()
        {
            var userId = GetUserId();

            var contacts = _contactService.GetUserContacts(userId)
                .Select(c => new ContactResponseDto
                {
                    Id = c.Id,
                    PhoneNumber = c.PhoneNumber,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    CreatedAt = c.CreatedAt
                });

            return Ok(contacts);
        }
        [HttpGet("search")]
        public IActionResult Search([FromQuery] string q)
        {
            var userId = GetUserId();

            var results = _contactService.SearchContacts(userId, q);

            if (!results.Any())
                return NotFound("Not Found");

            return Ok(results.Select(c => new ContactResponseDto
            {
                Id = c.Id,
                PhoneNumber = c.PhoneNumber,
                Name = c.Name,
                ImageUrl = c.ImageUrl
            }));
        }

    }
}
