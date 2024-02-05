using System.Net;
using Application.Milestones.Commands.Close;
using Application.Milestones.Commands.Create;
using Application.Milestones.Commands.Update;
using Application.Milestones.Commands.Delete;
using Application.Milestones.Commands.Reopen;
using Application.Milestones.Queries.FindRepositoryClosedMilestones;
using Application.Milestones.Queries.FindRepositoryMilestones;
using Domain.Milestones;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Milestones.Dtos;
using WEB_API.Milestones.Presenters;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;
using Application.Issues.Queries.FindIssueEventsQuery;
using Domain.Tasks;
using Application.Milestones.Queries.FindCompletionPercentageOfMilestone;

namespace WEB_API.Milestones;

[ApiController]
[Route("milestones")]
public class MilestoneController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;
    
    public MilestoneController(ISender sender,ITokenHandler tokenHandler,IUserIdentityService userIdentityService)
    {
        _sender = sender;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }

    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] MilestoneDto milestoneDto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var createdMilestoneId = await _sender.Send(new CreateMilestoneCommand(milestoneDto.RepositoryId, milestoneDto.Title, 
            milestoneDto.Description, creatorId, milestoneDto.DueDate));
        return Ok(createdMilestoneId);
    }

    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateMilestoneDto milestoneDto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var updatedMilestone = await _sender.Send(new UpdateMilestoneCommand(Guid.Parse(milestoneDto.Id),
            milestoneDto.Title, milestoneDto.Description, creatorId, milestoneDto.DueDate));
        return Ok(updatedMilestone);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        Guid userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Guid deletedMilestone = await _sender.Send(new DeleteMilestoneCommand(userId, Guid.Parse(id)));
        return Ok(deletedMilestone);
    }

    [HttpPut("{id}/close")]
    [Authorize]
    public async Task<IActionResult> Close(string id)
    {
        Guid userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Milestone closedMilestone = await _sender.Send(new CloseMilestoneCommand(userId, Guid.Parse(id)));
        return Ok(new MilestonePresenter(closedMilestone));
    }
    
    [HttpPut("{id}/reopen")]
    [Authorize]
    public async Task<IActionResult> Reopen(string id)
    {
        Guid userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Milestone reopenedMilestone = await _sender.Send(new ReopenMilestoneCommand(userId, Guid.Parse(id)));
        return Ok(new MilestonePresenter(reopenedMilestone));
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> FindRepositoryMilestones(string id)
    {
        Guid userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        List<Milestone> milestones = await _sender.Send(new FindRepositoryMilestonesQuery(userId, Guid.Parse(id)));
        var presenters = MilestonePresenter.MapFromMilestonesToMilestonePresenters(milestones);
        foreach (MilestonePresenter presenter in presenters)
        {
            presenter.CompletionPercentage = await _sender.Send(new FindCompletionPercentageOfMilestoneQuery(presenter.Id));
        }
        return Ok(presenters);
    }
    
    [HttpGet("{id}/closed")]
    [Authorize]
    public async Task<IActionResult> FindRepositoryClosedMilestones(string id)
    {
        Guid userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        List<Milestone> milestones = await _sender.Send(new FindRepositoryClosedMilestonesQuery(userId, Guid.Parse(id)));
        return Ok(MilestonePresenter.MapFromMilestonesToMilestonePresenters(milestones));
    }

    [HttpGet("completion-percentage/{id}")]
    [Authorize]
    public async Task<IActionResult> FindCompletionPercentageOfMilestone(Guid id)
    {
        double percentage = await _sender.Send(new FindCompletionPercentageOfMilestoneQuery(id));
        return Ok(percentage);
    }
}