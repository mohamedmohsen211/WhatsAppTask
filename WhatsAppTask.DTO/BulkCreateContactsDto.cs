namespace WhatsAppTask.DTO
{
    public class BulkCreateContactsDto
    {
        public List<BulkContactItemDto> Contacts { get; set; } = new();
    }
}
