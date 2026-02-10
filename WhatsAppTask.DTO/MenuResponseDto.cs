using WhatsAppTask.DTO;

public class MenuResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Message { get; set; } = null!;
    public bool IsRoot { get; set; }
    public List<MenuItemResponseDto> Items { get; set; } = new();
}