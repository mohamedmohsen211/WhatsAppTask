public class AutoReply
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Keyword { get; set; } = null!;
    public string Reply { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
