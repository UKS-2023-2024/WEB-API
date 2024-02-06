using Application.Issues.Commands.Create;
using Application.Issues.Queries.FindIssueQuery;
using Application.Issues.Queries.FindRepositoryIssues;
using Application.PullRequests.Commands;
using Application.PullRequests.Queries;
using Application.PullRequests.Queries.FindPullRequest;
using Application.PullRequests.Queries.FindPullRequestEvents;
using Application.PullRequests.Queries.FindRepositoryPullRequests;
using Domain.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Create([FromBody] PullRequestDto pullRequestDto)
    {
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var createdPullRequestId = await _sender.Send(new CreatePullRequestCommand(creatorId, pullRequestDto.Title, 
            pullRequestDto.Description, pullRequestDto.RepositoryId, pullRequestDto.AssigneesIds, pullRequestDto.LabelsIds,
            pullRequestDto.MilestoneId,pullRequestDto.FromBranchId, pullRequestDto.ToBranchId));
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

    [HttpGet("{repositoryId:guid}/pullRequests")]
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
}