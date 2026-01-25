using System;
using System.Collections.Generic;
using System.Text;
using WhatsAppTask.BLL.Interfaces;
using WhatsAppTask.DAL.DbContext;

namespace WhatsAppTask.BLL.Services
{
    public class AutoReplyService : IAutoReplyService
    {
        private readonly AppDbContext _context;

        public AutoReplyService(AppDbContext context)
        {
            _context = context;
        }

        public AutoReply Create(int userId, string keyword, string reply)
        {
            var autoReply = new AutoReply
            {
                UserId = userId,
                Keyword = keyword.ToLower(),
                Reply = reply,
                CreatedAt = DateTime.UtcNow
            };

            _context.AutoReplies.Add(autoReply);
            _context.SaveChanges();
            return autoReply;
        }

        public List<AutoReply> GetAll(int userId)
        {
            return _context.AutoReplies
                .Where(a => a.UserId == userId)
                .ToList();
        }

        public void Delete(int userId, int id)
        {
            var item = _context.AutoReplies
                .FirstOrDefault(a => a.Id == id && a.UserId == userId);

            if (item == null)
                throw new Exception("Auto reply not found");

            _context.AutoReplies.Remove(item);
            _context.SaveChanges();
        }

        public AutoReply? FindMatch(int userId, string message)
        {
            message = message.ToLower();

            return _context.AutoReplies
                .FirstOrDefault(a =>
                    a.UserId == userId &&
                    message.Contains(a.Keyword));
        }
    }
}
