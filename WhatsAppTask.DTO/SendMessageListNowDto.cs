namespace WhatsAppTask.DTO
{
    public class SendMessageListNowDto
    {
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public List<int> ContactIds { get; set; } = new();
    }
}
