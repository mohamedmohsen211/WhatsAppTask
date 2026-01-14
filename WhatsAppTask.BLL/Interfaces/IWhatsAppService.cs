namespace WhatsAppTask.BLL.Interfaces
{
    public interface IWhatsAppService
    {
        Task SendTextMessageAsync(string phoneNumber, string content);
        Task SendTemplateMessageAsync(string toPhoneNumber);
    }

}
