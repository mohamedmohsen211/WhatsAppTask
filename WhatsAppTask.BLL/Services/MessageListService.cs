using Microsoft.EntityFrameworkCore;
using WhatsAppTask.DAL.DbContext;

namespace WhatsAppTask.BLL.Services
{
    public class MessageListService
    {
        private readonly AppDbContext _context;
        private readonly IMessageService _messageService;

        public MessageListService(AppDbContext context, IMessageService messageService)
        {
            _context = context;
            _messageService = messageService;
        }

        public MessageList Create(int userId, string title, string message)
        {
            var list = new MessageList
            {
                UserId = userId,
                Title = title,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            _context.MessageLists.Add(list);
            _context.SaveChanges();
            return list;
        }
        public List<MessageList> GetAll(int userId)
        {
            return _context.MessageLists
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .ToList();
        }
        public MessageList GetById(int userId, int listId)
        {
            var list = _context.MessageLists
                .Include(l => l.Items)
                    .ThenInclude(i => i.Contact)
                .FirstOrDefault(l => l.Id == listId && l.UserId == userId);

            if (list == null)
                throw new Exception("Message list not found");

            return list;
        }

        public void AddItems(int listId, List<int> contactIds)
        {
            foreach (var contactId in contactIds)
            {
                _context.MessageListItems.Add(new MessageListItem
                {
                    MessageListId = listId,
                    ContactId = contactId
                });
            }

            _context.SaveChanges();
        }
        public async Task BulkSendAsync(int userId, int listId)
        {
            var list = _context.MessageLists
                .FirstOrDefault(l => l.Id == listId && l.UserId == userId);

            if (list == null)
                throw new Exception("List not found");

            var contacts = _context.MessageListItems
                .Where(i => i.MessageListId == listId)
                .Select(i => i.Contact)
                .ToList();

            foreach (var contact in contacts)
            {
                await _messageService.SendMessageAsync(
                    userId,
                    contact.PhoneNumber,
                    list.Message
                );
            }
        }
    }

}
