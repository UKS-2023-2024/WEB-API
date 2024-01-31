using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace WEB_API.Webhooks;


[ApiController]
[Route("/webhook")]
public class WebhookController: ControllerBase
{


    [HttpPost("")]
    public IActionResult WebhookHandler([FromBody] object data)
    {
        Console.WriteLine(JsonSerializer.Serialize(data));
        return Ok();
    }
}