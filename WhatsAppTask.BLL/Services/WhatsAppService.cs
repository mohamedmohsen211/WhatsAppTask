using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WhatsAppTask.BLL.Interfaces;

public class WhatsAppService : IWhatsAppService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public WhatsAppService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendTextMessageAsync(string toPhoneNumber, string message)
    {
        var token = _configuration["WhatsApp:AccessToken"];
        var phoneNumberId = _configuration["WhatsApp:PhoneNumberId"];
        var apiUrl = _configuration["WhatsApp:ApiUrl"];

        var url = $"{apiUrl}/{phoneNumberId}/messages";

        var body = new
        {
            messaging_product = "whatsapp",
            to = toPhoneNumber,
            type = "text",
            text = new
            {
                body = message
            }
        };

        await SendAsync(url, token, body);
    }

    public async Task SendTemplateMessageAsync(string toPhoneNumber)
    {
        var token = _configuration["WhatsApp:AccessToken"];
        var phoneNumberId = _configuration["WhatsApp:PhoneNumberId"];
        var apiUrl = _configuration["WhatsApp:ApiUrl"];

        var url = $"{apiUrl}/{phoneNumberId}/messages";

        var body = new
        {
            messaging_product = "whatsapp",
            to = toPhoneNumber,
            type = "template",
            template = new
            {
                name = "hello_world",
                language = new
                {
                    code = "en_US"
                }
            }
        };

        await SendAsync(url, token, body);
    }

    private async Task SendAsync(string url, string token, object body)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }
    }
}
