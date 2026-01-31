using WhatsAppTask.DAL.Entities;

public interface IMessageService
{
    Task<Message> SendMessageAsync(int userId, string phoneNumber, string content);

    List<Message> GetConversationMessages(int userId, int conversationId);
    List<ConversationWithLastMessageDto> GetConversationsWithLastMessage(int userId);
    Task SaveIncomingMessageAsync(int conversationId, string content);

}
