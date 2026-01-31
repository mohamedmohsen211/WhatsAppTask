using WhatsAppTask.DTO.Enums;

public class LastMessageDto
{
    public string Content { get; set; }
    public bool IsIncoming { get; set; }
    public MessageStatusDto Status { get; set; }
    public DateTime CreatedAt { get; set; }
}