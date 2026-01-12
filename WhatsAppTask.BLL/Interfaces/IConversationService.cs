using WhatsAppTask.DAL.Entities;

namespace WhatsAppTask.BLL.Interfaces
{
    public interface IConversationService
    {
        Conversation GetOrCreateByPhone(
            int userId,
            string phoneNumber,
            string? name,
            string? imageUrl
        );

        List<Conversation> GetUserConversations(int userId);
    }
}
