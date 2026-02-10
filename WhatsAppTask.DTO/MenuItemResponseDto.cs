namespace WhatsAppTask.DTO;
    public class MenuItemResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public MenuItemType Type { get; set; }
    public string? ReplyMessage { get; set; }
    public int? NextMenuId { get; set; }
}