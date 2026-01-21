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
            var userId = 1;

            var conversation = _conversationService.GetOrCreateByPhone(
                userId,
                request.From,
                null,
                null
            );

            await _messageService.SaveIncomingMessageAsync(
                conversation.Id,
                request.Text
            );

            await _whatsAppService.SendTextMessageAsync(
                request.From,
                "تم استلام رسالتك ✅"
            );
        }
        catch (Exception ex)
        {
        }
        return Ok();
    }
}
