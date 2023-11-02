using Application.Organizations.Commands.Create;
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
        await _sender.Send(new CreateOrganizationCommand(organizationDto.Name, organizationDto.ContactEmail,
            organizationDto.PendingMembers, creatorId));
        return Ok();
    }
}