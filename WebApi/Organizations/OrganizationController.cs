using Application.Organizations.Commands.AcceptInvite;
using Application.Organizations.Commands.Create;
using Application.Organizations.Commands.Delete;
using Application.Organizations.Commands.SendInvite;
using Application.Organizations.Queries.FindUserOrganizations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Organization.Dtos;
using WEB_API.Organization.Presenters;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;

namespace WEB_API.Organizations;


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
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        
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
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        
        await _sender.Send(new DeleteOrganizationCommand(Guid.Parse(id), userId));
        return Ok();
    }

    [HttpGet("member")]
    [Authorize]
    public async Task<IActionResult> FindUserOrganizations()
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);

        List<Domain.Organizations.Organization> organizations = await _sender.Send(new FindUserOrganizationsQuery(userId));
        return Ok(OrganizationPresenter.MapOrganizationsToOrganizationPresenters(organizations));
    }

    [HttpPost("{orgId}/members/{memberId}/invite")]
    public async Task<IActionResult> SendOrgInvitation(string orgId, string memberId)
    {
        var authorized = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new SendInviteCommand(authorized, new Guid(orgId), new Guid(memberId)));
        return Ok();
    }

    [HttpPost("/invite/${inviteId}")]
    public async Task<IActionResult> AcceptOrgInvitation(string inviteId)
    {
        var authorized = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new AcceptInviteCommand(authorized ,new Guid(inviteId)));
        return Ok();
    }
}