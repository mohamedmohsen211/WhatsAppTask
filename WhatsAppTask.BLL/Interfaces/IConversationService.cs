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
        public void DeleteConversation(int userId, int conversationId);
        public void BulkDelete(int userId, List<int> conversationIds);
    }
}
