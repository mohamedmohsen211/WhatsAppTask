using WhatsAppTask.DAL.Entities;

public interface IAdminService
{
    User Create(string email, string password);
    List<User> GetAll();
    User Update(int id, string email);
    void Delete(int id);
}