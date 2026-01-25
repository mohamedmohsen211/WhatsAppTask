using System;
using System.Collections.Generic;
using System.Text;

namespace WhatsAppTask.BLL.Interfaces
{
    public interface IAutoReplyService
    {
        AutoReply Create(int userId, string keyword, string reply);
        List<AutoReply> GetAll(int userId);
        void Delete(int userId, int id);
        AutoReply? FindMatch(int userId, string message);
    }
}
