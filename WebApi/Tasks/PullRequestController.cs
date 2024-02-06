using Application.Issues.Commands.Create;
using Application.PullRequests.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Shared.UserIdentityService;
using WEB_API.Tasks.Dtos;

namespace WEB_API.Tasks;

public class PullRequestController:ControllerBase
{
    private readonly ISender _sender;
    private readonly IUserIdentityService _userIdentityService;
    
    public PullRequestController(ISender sender,IUserIdentityService userIdentityService)
    {
        _sender = sender;
        _userIdentityService = userIdentityService;
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] PullRequestDto pullRequestDto)
    {
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var createdPullRequestId = await _sender.Send(new CreatePullRequestCommand(creatorId, pullRequestDto.Title, 
            pullRequestDto.Description, pullRequestDto.RepositoryId, pullRequestDto.AssigneesIds, pullRequestDto.LabelsIds,
            pullRequestDto.MilestoneId,pullRequestDto.FromBranchId, pullRequestDto.ToBranchId));
        return Ok(new {Id = createdPullRequestId});
    }
}