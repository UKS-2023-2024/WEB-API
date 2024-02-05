using System.Text.Json;
using Application.Branches.Commands.CreateFromWebhook;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WEB_API.Webhooks.Dtos;

namespace WEB_API.Webhooks;


[ApiController]
[Route("/webhook")]
public class WebhookController: ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger _logger;

    public WebhookController(ISender sender, ILogger<WebhookController> logger)
    {
        _sender = sender;
        _logger = logger;
    } 

    [HttpPost("")]
    public async Task<IActionResult> WebhookHandler([FromBody] PushWebhookDto data)
    {
        var log =
            "Webhook Triggered: Type: Push, Repository: " +data.repository?.name + ", Owner: "+ data.repository?.owner?.username + " Ref: " + data.@ref;
        _logger.LogInformation(log);
        await _sender.Send(new CreateBranchFromWebhookCommand(data.repository?.owner?.username, data.repository?.name,
            data.@ref));
        return Ok();
    }
}