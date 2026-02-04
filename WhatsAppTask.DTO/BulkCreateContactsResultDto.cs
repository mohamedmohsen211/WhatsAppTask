namespace WhatsAppTask.DTO
{
    public class BulkCreateContactsResultDto
    {
        public List<ContactResponseDto> Created { get; set; } = new();
        public List<string> Duplicates { get; set; } = new();
        public List<string> Invalid { get; set; } = new();
    }
}
