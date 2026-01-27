using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
    public async Task<IActionResult> ReceiveMessage([FromBody] JsonElement payload)
    {
        try
        {
            var entry = payload.GetProperty("entry")[0];
            var changes = entry.GetProperty("changes")[0];
            var value = changes.GetProperty("value");

            if (!value.TryGetProperty("messages", out var messages))
                return Ok();

            var message = messages[0];

            var from = message.GetProperty("from").GetString();
            var text = message
                .GetProperty("text")
                .GetProperty("body")
                .GetString();

            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(text))
                return Ok();

            var userId = 1;

            var conversation = _conversationService.GetOrCreateByPhone(
                userId,
                from,
                null,
                null
            );

            await _messageService.SaveIncomingMessageAsync(
                conversation.Id,
                text
            );

            var autoReply = _autoReplyService.FindMatch(userId, text);

            if (autoReply != null)
            {
                await _whatsAppService.SendTextMessageAsync(
                    from,
                    autoReply.Reply
                );
            }
        }
        catch (Exception ex)
        {
        }

        return Ok();
    }

}
