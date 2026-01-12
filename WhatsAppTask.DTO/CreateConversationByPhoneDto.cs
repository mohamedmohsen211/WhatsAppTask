namespace WhatsAppTask.DTO
{
    public class CreateConversationByPhoneDto
    {
        public string PhoneNumber { get; set; } = null!;
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
