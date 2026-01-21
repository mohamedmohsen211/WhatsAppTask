using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DAL.DbContext;
using WhatsAppTask.DAL.Entities;

namespace WhatsAppTask.BLL.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public Contact CreateContact(
            int userId,
            string phoneNumber,
            string? name,
            string? imageUrl)
        {
            phoneNumber = NormalizePhone(phoneNumber);

            var exists = _context.Contacts.Any(c =>
                c.UserId == userId &&
                c.PhoneNumber == phoneNumber);

            if (exists)
                throw new Exception("Contact already exists");

            var contact = new Contact
            {
                UserId = userId,
                PhoneNumber = phoneNumber,
                Name = name,
                ImageUrl = imageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            return contact;
        }

        private string NormalizePhone(string phone)
        {
            return phone
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Trim()
                .TrimStart('+');
        }


        public List<Contact> GetUserContacts(int userId)
        {
            return _context.Contacts
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }
        public List<Contact> SearchContacts(int userId, string query)
        {
            query = query.ToLower();

            return _context.Contacts
                .Where(c =>
                    c.UserId == userId &&
                    (
                        c.PhoneNumber.Contains(query) ||
                        (c.Name != null && c.Name.ToLower().Contains(query))
                    )
                )
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }

    }
}
