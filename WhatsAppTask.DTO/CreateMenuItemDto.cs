public class CreateMenuItemDto
{
    public string Title { get; set; } = null!;
    public MenuItemType Type { get; set; }
    public string? ReplyMessage { get; set; }
    public int? NextMenuId { get; set; }
}