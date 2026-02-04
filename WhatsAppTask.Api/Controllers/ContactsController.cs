using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
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

            if (!IsValidPhone(request.PhoneNumber))
            {
                return BadRequest(new
                {
                    message = "Invalid phone number format. Use country code, digits only."
                });
            }

            var existing = _contactService
                .GetUserContacts(userId)
                .Any(c => c.PhoneNumber ==
                    request.PhoneNumber
                        .Replace(" ", "")
                        .Replace("-", "")
                        .Replace("(", "")
                        .Replace(")", "")
                        .Trim()
                        .TrimStart('+')
                );

            if (existing)
            {
                return Conflict(new
                {
                    message = "Phone number already exists"
                });
            }

            var contact = _contactService.CreateContact(
                userId,
                request.PhoneNumber,
                request.Name,
                request.ImageUrl
            );

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
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var userId = GetUserId();

            var contact = _contactService.GetContactById(userId, id);

            if (contact == null)
                return NotFound("Contact not found");

            return Ok(new ContactResponseDto
            {
                Id = contact.Id,
                PhoneNumber = contact.PhoneNumber,
                Name = contact.Name,
                ImageUrl = contact.ImageUrl,
                CreatedAt = contact.CreatedAt
            });
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
        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\+?\d{10,15}$");
        }

        [HttpPost("bulk")]
        public IActionResult BulkCreate(BulkCreateContactsDto request)
        {
            var userId = GetUserId();

            if (request.Contacts == null || !request.Contacts.Any())
            {
                return BadRequest("Contacts array is required");
            }

            var result = _contactService.BulkCreateContacts(
                userId,
                request.Contacts
            );

            return Ok(result);
        }
    }
}
