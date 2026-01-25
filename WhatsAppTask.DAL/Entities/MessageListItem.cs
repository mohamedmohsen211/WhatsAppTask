using WhatsAppTask.DAL.Entities;

public class MessageListItem
{
    public int Id { get; set; }

    public int MessageListId { get; set; }
    public MessageList MessageList { get; set; } = null!;

    public int ContactId { get; set; }
    public Contact Contact { get; set; } = null!;
}