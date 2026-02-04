using System;

namespace WhatsAppTask.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }

        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;

        public string? Content { get; set; } = null!;
        public bool IsIncoming { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public MessageStatus Status { get; set; } = MessageStatus.Pending;
        public string? FailureReason { get; set; }
        public string? AttachmentUrl { get; set; }
        public MessageAttachmentType? AttachmentType { get; set; }

}
}
