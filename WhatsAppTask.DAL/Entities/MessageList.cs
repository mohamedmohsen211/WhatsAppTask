using WhatsAppTask.DAL.Entities;

public class MessageList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public List<MessageListItem> Items { get; set; } = new();
    public User User { get; set; }
}