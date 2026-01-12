using Microsoft.VisualBasic;
using System;

namespace WhatsAppTask.DAL.Entities
{
    public class Contact
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Conversation? Conversation { get; set; }
    }
}
