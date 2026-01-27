using Microsoft.EntityFrameworkCore;
using WhatsAppTask.DAL.Entities;

namespace WhatsAppTask.DAL.DbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<AutoReply> AutoReplies { get; set; }
        public DbSet<MessageList> MessageLists { get; set; }
        public DbSet<MessageListItem> MessageListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Contact>()
                .HasIndex(c => new {c.PhoneNumber })
                .IsUnique();


            modelBuilder.Entity<Conversation>()
                .HasIndex(c => new { c.UserId, c.ContactId })
                .IsUnique();

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Contact)
                .WithOne(c => c.Conversation)
                .HasForeignKey<Conversation>(c => c.ContactId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany()
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MessageList>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageListItem>()
                .HasOne(i => i.MessageList)
                .WithMany(l => l.Items)
                .HasForeignKey(i => i.MessageListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageListItem>()
                .HasOne(i => i.Contact)
                .WithMany()
                .HasForeignKey(i => i.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
