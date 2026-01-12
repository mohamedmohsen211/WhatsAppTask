using WhatsAppTask.DAL.Entities;

namespace WhatsAppTask.BLL.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}