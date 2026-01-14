using Microsoft.EntityFrameworkCore;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DAL.DbContext;
using WhatsAppTask.DAL.Entities;

public class MessageService : IMessageService
{
    private readonly AppDbContext _context;
    private readonly IWhatsAppService _whatsAppService;
    private readonly IConversationService _conversationService;

    public MessageService(
        AppDbContext context,
        IWhatsAppService whatsAppService,
        IConversationService conversationService)
    {
        _context = context;
        _whatsAppService = whatsAppService;
        _conversationService = conversationService;
    }

    public async Task<Message> SendMessageAsync(
    int userId,
    string phoneNumber,
    string content)
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
}
