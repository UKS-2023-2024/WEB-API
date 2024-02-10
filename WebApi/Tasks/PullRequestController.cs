using Application.Issues.Commands.Enums;
using Application.Issues.Commands.Update;
using Application.PullRequests.Commands;
using Application.PullRequests.Commands.Close;
using Application.PullRequests.Commands.IssueAssignment;
using Application.PullRequests.Commands.MilestoneAssignment;
using Application.PullRequests.Commands.MilestoneUnassignment;
using Application.PullRequests.Commands.Reopen;
using Application.PullRequests.Queries.FindPullRequest;
using Application.PullRequests.Queries.FindPullRequestEvents;
using Application.PullRequests.Queries.FindRepositoryPullRequests;
using Domain.Milestones;
using Domain.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Milestones.Presenters;
using WEB_API.Shared.UserIdentityService;
using WEB_API.Tasks.Dtos;
using WEB_API.Tasks.Presenters;

namespace WEB_API.Tasks;


[ApiController]
[Route("pull-requests")]
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
    public async Task<IActionResult> Create([FromBody] CreatePullRequestDto createPullRequestDto)
    {
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var createdPullRequestId = await _sender.Send(new CreatePullRequestCommand(creatorId, createPullRequestDto.Title, 
            createPullRequestDto.Description, createPullRequestDto.RepositoryId, createPullRequestDto.AssigneesIds, createPullRequestDto.LabelsIds,
            createPullRequestDto.MilestoneId,createPullRequestDto.FromBranchId, createPullRequestDto.ToBranchId, createPullRequestDto.IssueIds));
        return Ok(new {Id = createdPullRequestId});
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> FindPullRequestById(Guid id)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var pullRequest = await _sender.Send(new FindPullRequestQuery(id));
        return Ok(new PullRequestPresenter(pullRequest));
    }

    [HttpGet("{repositoryId}/pull-requests")]
    [Authorize]
    public async Task<IActionResult> FindRepositoryPullRequests(Guid repositoryId)
    {
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var pullRequests = await _sender.Send(new FindRepositoryPullRequestsQuery(creatorId, repositoryId));
        return Ok(PullRequestPresenter.MapPullRequestToPullRequestPresenter(pullRequests));
    }
    
    [HttpGet("{pullRequestId:Guid}/events")]
    [Authorize]
    public async Task<IActionResult> FindPullRequestEvents(Guid pullRequestId)
    {
        var events = await _sender.Send(new FindPullRequestEventsQuery(pullRequestId));
        return Ok(events);
    }

    [HttpPut("close/{id}")]
    [Authorize]
    public async Task<IActionResult> Close(Guid id)
    {
        Guid userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        PullRequest pr = await _sender.Send(new ClosePullRequestCommand(userId, id));
        return Ok(new PullRequestPresenter(pr));
    }


    [HttpPut("reopen/{id}")]
    [Authorize]
    public async Task<IActionResult> Reopen(Guid id)
    {
        Guid userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        PullRequest reopenedPr = await _sender.Send(new ReopenPullRequestCommand(userId, id));
        return Ok(new PullRequestPresenter(reopenedPr));
    }

    [HttpPut("issues/update")]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdatePullRequestDto dto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Guid updatedPrGuid = await _sender.Send(new AssignIssuesToPullRequestCommand(dto.Id, 
            creatorId, dto.IssueIds));
        return Ok(updatedPrGuid);
    }

    [HttpPut("milestone/update")]
    [Authorize]
    public async Task<IActionResult> UpdateMilestone([FromBody] UpdatePullRequestDto dto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Guid updatedPrGuid = await _sender.Send(new AssignMilestoneToPullRequestCommand(dto.Id,
            creatorId, dto.MilestoneId));
        return Ok(updatedPrGuid);
    }


    [HttpPut("unassign/milestone")]
    [Authorize]
    public async Task<IActionResult> UnassignMilestone([FromBody] UpdatePullRequestDto dto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Guid updatedPrGuid = await _sender.Send(new UnassignMilestoneToPullRequestCommand(dto.Id,
            creatorId));
        return Ok(updatedPrGuid);
    }

}