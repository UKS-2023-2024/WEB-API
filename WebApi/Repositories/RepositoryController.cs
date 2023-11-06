using Application.Auth.Commands.Update;
using Application.Auth.Queries.FindAll;
using Application.Organizations.Commands.Delete;
using Application.Repositories.Commands.Create;
using Application.Repositories.Commands.Create.CreateForOrganization;
using Application.Repositories.Commands.Create.CreateForUser;
using Application.Repositories.Commands.Delete;
using Application.Repositories.Queries.FindAllByOrganizationId;
using Application.Repositories.Queries.FindAllByOwnerId;
using Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Repositories.Dtos;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;

namespace WEB_API.Repositories;


[ApiController]
[Route("repositories")]
public class RepositoryController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;

    public RepositoryController(ISender sender,ITokenHandler tokenHandler,IUserIdentityService userIdentityService)
    {
        _sender = sender;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }

    [HttpPost("user")]
    [Authorize]
    public async Task<IActionResult> CreateForUser([FromBody] UserRepositoryDto repositoryDto)
    {
        string? creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        if (creatorId is null)
            return Unauthorized();

        var createdRepositoryId = await _sender.Send(new CreateRepositoryForUserCommand(repositoryDto.Name, repositoryDto.Description, 
            repositoryDto.IsPrivate, Guid.Parse(creatorId)));

        return Ok(createdRepositoryId);
    }


    [HttpPost("organization")]
    [Authorize]
    public async Task<IActionResult> CreateForOrganization([FromBody] OrganizationRepositoryDto dto)
    {
        string? creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        if (creatorId is null)
            return Unauthorized();

        var createdRepositoryId = await _sender.Send(new CreateRepositoryForOrganizationCommand(dto.Name, dto.Description,
            dto.IsPrivate, Guid.Parse(creatorId), dto.OrganizationId));

        return Ok(createdRepositoryId);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        string? userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        if (userId is null)
            return Unauthorized();

        await _sender.Send(new DeleteRepositoryCommand(Guid.Parse(userId), Guid.Parse(id)));

        return Ok();
    }


    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateRepositoryDto dto)
    {
        string? userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        if (userId is null)
            return Unauthorized();

        await _sender.Send(new UpdateRepositoryCommand(Guid.Parse(userId), dto.Id, dto.Name, dto.Description, dto.IsPrivate));

        return Ok();
    }

    [HttpGet("{ownerId}")]
    [Authorize]
    public async Task<IActionResult> FindAllByOwnerId(Guid ownerId)
    {
        var repositories = await _sender.Send(new FindAllRepositoriesByOwnerIdQuery(ownerId));
        return Ok(repositories);
    }

    [HttpGet("organization/{organizationId}")]
    [Authorize]
    public async Task<IActionResult> FindAllByOrganizationIdAsync(Guid organizationId)
    {
        var repositories = await _sender.Send(new FindAllRepositoriesByOrganizationIdQuery(organizationId));
        return Ok(repositories);
    }

}