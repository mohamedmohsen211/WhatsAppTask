using WhatsAppTask.DAL.Entities;
using WhatsAppTask.DTO;

namespace WhatsAppTask.BLL.Interfaces
{
    public interface IContactService
    {
        Contact CreateContact(int userId, string phoneNumber, string? name, string? imageUrl);
        List<Contact> GetUserContacts(int userId);
        List<Contact> SearchContacts(int userId, string query);
        Contact? GetContactById(int userId, int contactId);
        BulkCreateContactsResultDto BulkCreateContacts(int userId,List<BulkContactItemDto> contacts);
    }
}
