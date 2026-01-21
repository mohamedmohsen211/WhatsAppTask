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
            phoneNumber = NormalizePhone(phoneNumber);

            var existingContact = _context.Contacts
                .FirstOrDefault(c =>
                    c.UserId == userId &&
                    c.PhoneNumber == phoneNumber);

            if (existingContact != null)
                throw new Exception("This phone number already exists in your contacts");

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

            var conversation = new Conversation
            {
                UserId = userId,
                ContactId = contact.Id,
                CreatedAt = DateTime.UtcNow
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
        public void DeleteConversation(int userId, int conversationId)
        {
            var conversation = _context.Conversations
                .FirstOrDefault(c => c.Id == conversationId && c.UserId == userId);

            if (conversation == null)
                throw new Exception("Conversation not found");

            _context.Conversations.Remove(conversation);
            _context.SaveChanges();
        }

        public void BulkDelete(int userId, List<int> conversationIds)
        {
            var conversations = _context.Conversations
                .Where(c => c.UserId == userId && conversationIds.Contains(c.Id))
                .ToList();

            _context.Conversations.RemoveRange(conversations);
            _context.SaveChanges();
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

    }
}
