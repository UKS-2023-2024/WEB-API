using Application.Organizations.Commands.AcceptInvite;
using Application.Organizations.Commands.Create;
using Application.Organizations.Commands.Delete;
using Application.Organizations.Commands.SendInvite;
using Application.Organizations.Queries.FindUserOrganizations;
using Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Organization.Dtos;
using WEB_API.Organization.Presenters;
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
        List<Guid> pendingMembersGuids = new List<Guid>();
        foreach (string id in organizationDto.PendingMembers)
        {
            pendingMembersGuids.Add(Guid.Parse(id));
        }

        var createdOrgId = await _sender.Send(new CreateOrganizationCommand(organizationDto.Name, organizationDto.ContactEmail,
            pendingMembersGuids, creatorId));
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

    [HttpGet("member")]
    [Authorize]
    public async Task<IActionResult> FindUserOrganizations()
    {
        string? userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        
        if (userId is null)
            return Unauthorized();

        List<Domain.Organizations.Organization> organizations = await _sender.Send(new FindUserOrganizationsQuery(Guid.Parse(userId)));
        return Ok(OrganizationPresenter.MapOrganizationsToOrganizationPresenters(organizations));
    }

    [HttpPost("{orgId}/members/{memberId}/invite")]
    public async Task<IActionResult> SendOrgInvitation(string orgId, string memberId)
    {
        string authorized = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new SendInviteCommand(new Guid(authorized), new Guid(orgId), new Guid(memberId)));
        return Ok();
    }

    [HttpPost("/accept-invite")]
    public async Task<IActionResult> AcceptOrgInvitation([FromQuery] string token)
    {
        await _sender.Send(new AcceptInviteCommand(token));
        return Ok();
    }
}