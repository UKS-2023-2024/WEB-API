using Microsoft.AspNetCore.Mvc;

namespace WEB_API.Search;

[ApiController]
[Route("search")]
public class SearchController: ControllerBase
{
    public SearchController()
    {
    }
    
    [HttpPost("general/{query}")]
    public async Task<IActionResult> SearchGeneral(string query)
    {
        return Ok();
    }
    
    [HttpPost("tasks/{query}")]
    public async Task<IActionResult> SearchTaks(string query)
    {
        return Ok();
    }
}