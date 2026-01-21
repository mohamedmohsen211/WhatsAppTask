using Microsoft.AspNetCore.Mvc;
using WhatsAppTask.BLL.Interfaces;

[ApiController]
[Route("api/webhooks/whatsapp")]
public class WebhooksController : ControllerBase
{
    private readonly IConversationService _conversationService;
    private readonly IMessageService _messageService;
    private readonly IWhatsAppService _whatsAppService;

    public WebhooksController(
        IConversationService conversationService,
        IMessageService messageService,
        IWhatsAppService whatsAppService)
    {
        _conversationService = conversationService;
        _messageService = messageService;
        _whatsAppService = whatsAppService;
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveMessage([FromBody] WebhookMessageDto request)
    {
        try
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.From) ||
                string.IsNullOrWhiteSpace(request.Text))
                return Ok();

            var userId = 1;

            await _messageService.SendMessageAsync(
                userId,
                request.From,
                request.Text
            );
        }
        catch
        {
        }

        return Ok();
    }

}
