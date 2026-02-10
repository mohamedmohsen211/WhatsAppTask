public class Menu
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Message { get; set; } = null!;

    public bool IsRoot { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<MenuItem> Items { get; set; } = new();
}