using Microsoft.EntityFrameworkCore;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DAL.DbContext;
using WhatsAppTask.DAL.Entities;

public class MessageService : IMessageService
{
    private readonly AppDbContext _context;
    private readonly IWhatsAppService _whatsAppService;

    public MessageService(
        AppDbContext context,
        IWhatsAppService whatsAppService)
    {
        _context = context;
        _whatsAppService = whatsAppService;
    }

    public async Task<Message> SendMessageAsync(
        int userId,
        string phoneNumber,
        string content)
    {
        phoneNumber = NormalizePhone(phoneNumber);

        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.PhoneNumber == phoneNumber);

        if (contact == null)
        {
            contact = new Contact
            {
                UserId = userId,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        var conversation = await _context.Conversations
            .FirstOrDefaultAsync(c =>
                c.UserId == userId &&
                c.ContactId == contact.Id);

        if (conversation == null)
        {
            conversation = new Conversation
            {
                UserId = userId,
                ContactId = contact.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
        }

        var message = new Message
        {
            ConversationId = conversation.Id,
            Content = content,
            IsIncoming = false,
            Status = MessageStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        try
        {
            await _whatsAppService.SendTextMessageAsync(phoneNumber, content);
            message.Status = MessageStatus.Sent;
        }
        catch (Exception ex)
        {
            message.Status = MessageStatus.Failed;
            message.FailureReason = ex.Message;
        }

        await _context.SaveChangesAsync();

        return message;
    }


    public async Task SaveIncomingMessageAsync(int conversationId, string content)
    {
        var message = new Message
        {
            ConversationId = conversationId,
            Content = content,
            IsIncoming = true,
            Status = MessageStatus.Sent,
            CreatedAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
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
