using Application.Auth.Queries.FindAllByStarredRepository;
using Application.Auth.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Users.Dtos;

namespace WEB_API.Users;

[ApiController]
[Route("users")]
public class UserController: ControllerBase
{
    private readonly ISender _sender;
    public UserController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("search")]
    [Authorize]
    public async Task<IActionResult> Search([FromQuery] string value)
    {
        var users = await _sender.Send(new SearchUsersQuery(value));
        return Ok(UserDto.MapUsersToUserDtos(users));
    }
    
    [HttpGet("starred/{repositoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> Search(Guid repositoryId)
    {
        var users = await _sender.Send(new FindAllByStarredRepositoryQuery(repositoryId));
        return Ok(UserDto.MapUsersToUserDtos(users));
    }
}