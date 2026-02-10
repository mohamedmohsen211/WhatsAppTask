public enum MenuItemType
{
    Message = 1,
    Menu = 2
}

public class MenuItem
{
    public int Id { get; set; }

    public int MenuId { get; set; }
    public Menu Menu { get; set; } = null!;

    public string Title { get; set; } = null!;

    public MenuItemType Type { get; set; }

    public string? ReplyMessage { get; set; }

    public int? NextMenuId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}