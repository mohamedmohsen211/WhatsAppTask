using Microsoft.AspNetCore.Mvc;
using WhatsAppTask.BLL.Interfaces;

[ApiController]
[Route("api/webhooks/whatsapp")]
public class WebhooksController : ControllerBase
{
    private readonly IConversationService _conversationService;
    private readonly IMessageService _messageService;
    private readonly IWhatsAppService _whatsAppService;
    private readonly IAutoReplyService _autoReplyService;

    public WebhooksController(
        IConversationService conversationService,
        IMessageService messageService,
        IWhatsAppService whatsAppService,
        IAutoReplyService autoReplyService)
    {
        _conversationService = conversationService;
        _messageService = messageService;
        _whatsAppService = whatsAppService;
        _autoReplyService = autoReplyService;
    }

    [HttpPost]
    public async Task<IActionResult> ReceiveMessage([FromBody] WebhookMessageDto request)
    {
        if (request == null ||
            string.IsNullOrWhiteSpace(request.From) ||
            string.IsNullOrWhiteSpace(request.Text))
            return Ok();

        var userId = 1; // لحد ما نربطها صح بعدين

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

        var autoReply = _autoReplyService.FindMatch(userId, request.Text);

        if (autoReply != null)
        {
            await _whatsAppService.SendTextMessageAsync(
                request.From,
                autoReply.Reply
            );
        }

        return Ok();
    }
}
