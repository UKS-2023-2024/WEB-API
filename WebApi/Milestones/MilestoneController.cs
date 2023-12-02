using System.Net;
using Application.Milestones.Commands.Create;
using Application.Milestones.Commands.Update;
using Application.Milestones.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Milestones.Dtos;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;

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
        Guid closedMilestoneId = await _sender.Send(new CloseMilestoneCommand(userId, Guid.Parse(id)));
        return Ok(closedMilestoneId);
    }
}