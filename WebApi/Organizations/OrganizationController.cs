using Application.Organizations.Commands.AcceptInvite;
using Application.Organizations.Commands.ChangeOrganizationMemberRole;
using Application.Organizations.Commands.Create;
using Application.Organizations.Commands.Delete;
using Application.Organizations.Commands.RemoveOrganizationMember;
using Application.Organizations.Commands.SendInvite;
using Application.Organizations.Queries.FindOrganizationMemberRole;
using Application.Organizations.Queries.FindOrganizationMembers;
using Application.Organizations.Queries.FindUserOrganizations;
using Domain.Organizations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Organization.Dtos;
using WEB_API.Organization.Presenters;
using WEB_API.Organizations.Presenters;
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
    
    [HttpGet("{organizationId:Guid}/members")]
    [Authorize]
    public async Task<IActionResult> FindAllOrganizationMembers(Guid organizationId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);

        var organizations = await _sender.Send(new FindOrganizationMembersQuery(userId,organizationId));
        return Ok(OrganizationMemberPresenter.MapOrganizationMembersToOrganizationMemberPresenters(organizations));
    }

    [HttpPost("{orgId}/members/{memberId}/invite")]
    public async Task<IActionResult> SendOrgInvitation(string orgId, string memberId)
    {
        var authorized = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new SendInviteCommand(authorized, new Guid(orgId), new Guid(memberId)));
        return Ok();
    }
    
    [HttpDelete("{organizationId:guid}/members/{memberId:guid}")]
    public async Task<IActionResult> RemoveOrganizationMember(Guid organizationId, Guid memberId)
    {
        var ownerId = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new RemoveOrganizationMemberCommand(ownerId, memberId, organizationId));
        return Ok();
    }

    [HttpPost("invite/{inviteId}")]
    public async Task<IActionResult> AcceptOrgInvitation(string inviteId)
    {
        await _sender.Send(new AcceptInviteCommand(new Guid(inviteId)));
        return Ok();
    }
    
    [HttpPatch("change-user-role/{repositoryId:guid}/{organizationMemberId:guid}/{role}")]
    [Authorize]
    public async Task<IActionResult> ChangeOrganizationMemberRole(Guid repositoryId, Guid organizationMemberId,OrganizationMemberRole role)
    {
        var ownerId = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new ChangeOrganizationMemberRoleCommand(ownerId, organizationMemberId, repositoryId, role));
        return Ok();
    }
    
        
    [HttpGet("{organizationId:guid}/member-role")]
    [Authorize]
    public async Task<IActionResult> GetUserRole(Guid organizationId)
    {
        var user = await _userIdentityService.FindUserFromToken(HttpContext.User);
        if (user is null)
            return Unauthorized();
        var role = await _sender.Send(new FindOrganizationMemberRoleQuery(user.Id,organizationId));
        return Ok(role);
    }
}