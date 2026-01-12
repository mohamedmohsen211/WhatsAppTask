using WhatsAppTask.DAL.Entities;

public interface IMessageService
{
    Message SendMessage(int userId, string phoneNumber, string content);
    List<Message> GetConversationMessages(int userId, int conversationId);
}
