using Application.Issues.Commands.Create;
using Domain.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;
using WEB_API.Tasks.Dtos;

namespace WEB_API.Tasks;

[ApiController]
[Route("issues")]
public class IssueController: ControllerBase
{
    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;
    
    public IssueController(ISender sender,ITokenHandler tokenHandler,IUserIdentityService userIdentityService)
    {
        _sender = sender;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }

    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] IssueDto issueDto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Guid? milestoneId = issueDto.MilestoneId is not null ? Guid.Parse(issueDto.MilestoneId) : null;
        var createdIssueId = await _sender.Send(new CreateIssueCommand(creatorId, issueDto.Title, 
            issueDto.Description, Guid.Parse(issueDto.RepositoryId), issueDto.Assignees, issueDto.Labels,
            milestoneId));
        return Ok(createdIssueId);
    }


}