namespace WhatsAppTask.DTO
{
    public class ContactResponseDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
