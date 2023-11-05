using Application.Organizations.Commands.Create;
using Application.Organizations.Commands.Delete;
using Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Organization.Dtos;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;

namespace WEB_API.Organization;


[ApiController]
[Route("organizations")]
public class OrganizationController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;

    public OrganizationController(ISender sender,ITokenHandler tokenHandler,IUserIdentityService userIdentityService)
    {
        _sender = sender;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] OrganizationDto organizationDto)
    {
        var idString = _userIdentityService.FindUserIdentity(HttpContext.User);
        
        if (idString == null)
            return Unauthorized();
        
        var creatorId = Guid.Parse(idString);
        var createdOrgId = await _sender.Send(new CreateOrganizationCommand(organizationDto.Name, organizationDto.ContactEmail,
            organizationDto.PendingMembers, creatorId));
        return Ok(createdOrgId);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        string? userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        
        if (userId is null)
            return Unauthorized();
        await _sender.Send(new DeleteOrganizationCommand(Guid.Parse(id), Guid.Parse(userId)));
        return Ok();
    }
}