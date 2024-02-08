using Application.Issues.Commands.Create;
using Application.Issues.Queries.FindIssueQuery;
using Application.Issues.Queries.FindRepositoryIssues;
using Application.Milestones.Commands.Close;
using Application.PullRequests.Commands;
using Application.PullRequests.Commands.Close;
using Application.PullRequests.Queries;
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
    
    [HttpGet("{repositoryId:guid}/{pullRequestId:Guid}")]
    [Authorize]
    public async Task<IActionResult> FindPullRequestById(Guid pullRequestId, Guid repositoryId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var pullRequest = await _sender.Send(new FindPullRequestQuery(userId, pullRequestId, repositoryId));
        return Ok(new PullRequestPresenter(pullRequest));
    }

    [HttpGet("{repositoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> FindRepositoryPullRequests(Guid repositoryId)
    {
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var pullRequests = await _sender.Send(new FindRepositoryPullRequestsQuery(creatorId, repositoryId));
        return Ok(PullRequestPresenter.MapPullRequestToPullRequestPresenter(pullRequests));
    }
    
    [HttpGet("{repositoryId:guid}/{pullRequestId:Guid}/events")]
    [Authorize]
    public async Task<IActionResult> FindPullRequestEvents(Guid pullRequestId, Guid repositoryId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var events = await _sender.Send(new FindPullRequestEventsQuery(userId, pullRequestId, repositoryId));
        return Ok(events);
    }

    [HttpPut("{id}/close")]
    [Authorize]
    public async Task<IActionResult> Close(Guid id)
    {
        Guid userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        PullRequest pr = await _sender.Send(new ClosePullRequestCommand(userId, id));
        return Ok(new PullRequestPresenter(pr));
    }
}