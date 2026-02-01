using System;

namespace WhatsAppTask.DAL.Entities
{
    public class Conversation
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ContactId { get; set; }
        public Contact Contact { get; set; } = null!;
        public List<Message> Messages { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
