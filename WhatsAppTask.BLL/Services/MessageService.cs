using Microsoft.EntityFrameworkCore;
using WhatsAppTask.DAL.DbContext;
using WhatsAppTask.DAL.Entities;

public class MessageService : IMessageService
{
    private readonly AppDbContext _context;

    public MessageService(AppDbContext context)
    {
        _context = context;
    }

    public Message SendMessage(int userId, string phoneNumber, string content)
    {
        var conversation = _context.Conversations
            .Include(c => c.Contact)
            .FirstOrDefault(c =>
                c.UserId == userId &&
                c.Contact.PhoneNumber == phoneNumber);

        if (conversation == null)
            throw new Exception("Conversation not found");

        var message = new Message
        {
            ConversationId = conversation.Id,
            Content = content,
            IsIncoming = false
        };

        _context.Messages.Add(message);
        _context.SaveChanges();

        return message;
    }

    public List<Message> GetConversationMessages(int userId, int conversationId)
    {
        return _context.Messages
            .Include(m => m.Conversation)
            .Where(m =>
                m.ConversationId == conversationId &&
                m.Conversation.UserId == userId)
            .OrderBy(m => m.CreatedAt)
            .ToList();
    }
}
