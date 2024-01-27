using Application.Issues.Commands.Create;
using Application.Issues.Commands.Enums;
using Application.Issues.Commands.Update;
using Application.Issues.Queries.FindIssueQuery;
using Application.Issues.Queries.FindRepositoryIssues;
using Domain.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;
using WEB_API.Tasks.Dtos;
using WEB_API.Tasks.Presenters;

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
            issueDto.Description, Guid.Parse(issueDto.RepositoryId), issueDto.AssigneesIds, issueDto.LabelsIds,
            milestoneId));
        return Ok(new {Id = createdIssueId});
    }

    [HttpPost("assignee/update")]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateIssueDto issueDto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Guid milestoneId;
        Guid.TryParse(issueDto.MilestoneId, out milestoneId);
        Guid updatedIssueGuid = await _sender.Send(new UpdateIssueCommand(Guid.Parse(issueDto.Id), creatorId,
            issueDto.Title, issueDto.Description, issueDto.State, issueDto.Number,
            Guid.Parse(issueDto.RepositoryId), issueDto.AssigneesIds, issueDto.LabelsIds,
            UpdateIssueFlag.ASSIGNEES, milestoneId));
        return Ok(new {Id = updatedIssueGuid});
    }
    
    [HttpPost("milestone/update")]
    [Authorize]
    public async Task<IActionResult> UpdateMilestone([FromBody] UpdateIssueDto issueDto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Guid milestoneId;
        Guid.TryParse(issueDto.MilestoneId, out milestoneId);
        Guid updatedIssueGuid = await _sender.Send(new UpdateIssueCommand(Guid.Parse(issueDto.Id), creatorId,
            issueDto.Title, issueDto.Description, issueDto.State, issueDto.Number,
            Guid.Parse(issueDto.RepositoryId), issueDto.AssigneesIds, issueDto.LabelsIds,
            UpdateIssueFlag.MILESTONE_ASSIGNED, milestoneId));
        return Ok(new {Id = updatedIssueGuid});
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> FindById(string id)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Issue issue = await _sender.Send(new FindIssueQuery(creatorId, Guid.Parse(id)));
        foreach (var e in issue.Events)
        {
            Console.WriteLine(e.GetType());
        }
        return Ok(new IssuePresenter(issue));
    }

    [HttpGet("{repositoryId}/issues")]
    [Authorize]
    public async Task<IActionResult> FindRepositoryIssues(string repositoryId)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        List<Issue> issues = await _sender.Send(new FindRepositoryIssuesQuery(creatorId, Guid.Parse(repositoryId)));
        return Ok(IssuePresenter.MapIssueToIssuePresenter(issues));
    }
}