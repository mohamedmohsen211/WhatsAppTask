public class ConversationWithLastMessageDto
{
    public int ConversationId { get; set; }
    public int ContactId { get; set; }

    public LastMessageDto? LastMessage { get; set; }
}