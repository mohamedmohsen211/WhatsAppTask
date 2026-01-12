using Microsoft.EntityFrameworkCore;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DAL.DbContext;
using WhatsAppTask.DAL.Entities;

namespace WhatsAppTask.BLL.Services
{
    public class ConversationService : IConversationService
    {
        private readonly AppDbContext _context;

        public ConversationService(AppDbContext context)
        {
            _context = context;
        }

        public Conversation GetOrCreateByPhone(
            int userId,
            string phoneNumber,
            string? name,
            string? imageUrl)
        {
            var contact = _context.Contacts
                .FirstOrDefault(c =>
                    c.UserId == userId &&
                    c.PhoneNumber == phoneNumber);

            if (contact == null)
            {
                contact = new Contact
                {
                    UserId = userId,
                    PhoneNumber = phoneNumber,
                    Name = name,
                    ImageUrl = imageUrl
                };

                _context.Contacts.Add(contact);
                _context.SaveChanges();
            }

            var conversation = _context.Conversations
                .FirstOrDefault(c =>
                    c.UserId == userId &&
                    c.ContactId == contact.Id);

            if (conversation != null)
                return conversation;

            conversation = new Conversation
            {
                UserId = userId,
                ContactId = contact.Id
            };

            _context.Conversations.Add(conversation);
            _context.SaveChanges();

            return conversation;
        }

        public List<Conversation> GetUserConversations(int userId)
        {
            return _context.Conversations
                .Include(c => c.Contact)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();
        }
    }
}
