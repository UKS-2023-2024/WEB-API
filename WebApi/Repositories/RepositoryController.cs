﻿using Application.Auth.Commands.Update;
using Application.Repositories.Commands.Create.CreateForOrganization;
using Application.Repositories.Commands.Create.CreateForUser;
using Application.Repositories.Commands.Delete;
using Application.Repositories.Commands.HandleRepositoryMembers.AddRepositoryMember;
using Application.Repositories.Commands.StarringRepository.StarRepository;
using Application.Repositories.Commands.StarringRepository.UnstarRepository;
using Application.Repositories.Queries.FindAllByOrganizationId;
using Application.Repositories.Queries.FindAllByOwnerId;
using Domain.Repositories;
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
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);

        var createdRepositoryId = await _sender.Send(new CreateRepositoryForUserCommand(repositoryDto.Name, repositoryDto.Description, 
            repositoryDto.IsPrivate, creatorId));

        return Ok(createdRepositoryId);
    }


    [HttpPost("organization")]
    [Authorize]
    public async Task<IActionResult> CreateForOrganization([FromBody] OrganizationRepositoryDto dto)
    {
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);

        var createdRepositoryId = await _sender.Send(new CreateRepositoryForOrganizationCommand(dto.Name, dto.Description,
            dto.IsPrivate, creatorId, dto.OrganizationId));

        return Ok(createdRepositoryId);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);

        await _sender.Send(new DeleteRepositoryCommand(userId, Guid.Parse(id)));

        return Ok();
    }


    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateRepositoryDto dto)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Repository repository = await _sender.Send(new UpdateRepositoryCommand(userId, dto.Id, dto.Name, dto.Description, dto.IsPrivate));

        return Ok(repository);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> FindLoggedUserRepositories()
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var repositories = await _sender.Send(new FindAllRepositoriesByOwnerIdQuery(userId));
        return Ok(repositories);
    }
    
    [HttpPatch("star/{repositoryId}")]
    [Authorize]
    public async Task<IActionResult> StarRepository(Guid repositoryId)
    {
        var user = await _userIdentityService.FindUserFromToken(HttpContext.User);
        if (user is null)
            return Unauthorized();
        await _sender.Send(new StarRepositoryCommand(user,repositoryId));
        return Ok();
    }
    
    [HttpPatch("unstar/{repositoryId}")]
    [Authorize]
    public async Task<IActionResult> UnstarRepository(Guid repositoryId)
    {
        var user = await _userIdentityService.FindUserFromToken(HttpContext.User);
        if (user is null)
            return Unauthorized();
        await _sender.Send(new UnstarRepositoryCommand(user,repositoryId));
        return Ok();
    }

    [HttpPost("send-invite/{repositoryId:guid}/{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> SendRepositoryInvite(Guid repositoryId, Guid userId)
    {
        var user = await _userIdentityService.FindUserFromToken(HttpContext.User);
        if (user is null)
            return Unauthorized();
        await _sender.Send(new UnstarRepositoryCommand(user,repositoryId));
        return Ok();
    }
    
    [HttpPost("add-user/{inviteId:guid}")]
    [Authorize]
    public async Task<IActionResult> AddUserToRepository(Guid inviteId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new AddRepositoryMemberCommand(userId, inviteId));
        return Ok();
    }
    

    [HttpGet("organization/{organizationId}")]
    [Authorize]
    public async Task<IActionResult> FindAllByOrganizationIdAsync(Guid organizationId)
    {
        var repositories = await _sender.Send(new FindAllRepositoriesByOrganizationIdQuery(organizationId));
        return Ok(repositories);
    }

}