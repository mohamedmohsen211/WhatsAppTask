public class MessageResponseDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsIncoming { get; set; }
    public DateTime CreatedAt { get; set; }
}
