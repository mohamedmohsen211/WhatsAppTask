namespace WhatsAppTask.DTO
{
    public class ConversationResponseDto
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string PhoneNumber { get; set; }
        public string? ContactName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
