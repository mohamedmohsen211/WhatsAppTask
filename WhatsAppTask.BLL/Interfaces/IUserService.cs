using WhatsAppTask.DAL.Entities;

namespace WhatsAppTask.BLL.Interfaces
{
    public interface IUserService
    {
        User? Login(string usernameOrEmail, string password);
    }
}
