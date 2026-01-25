namespace WhatsAppTask.BLL.Interfaces
{
    public interface IMessageListService
    {
        MessageList CreateList(
            int userId,
            string title,
            string messageContent
        );
        void AddContactsToList(
            int listId,
            List<int> contactIds
        );
        List<MessageList> GetUserLists(int userId);
        MessageList GetListById(int userId, int listId);
        void DeleteList(int userId, int listId);
        Task BulkSendAsync(
            int userId,
            int listId
        );
    }
}
