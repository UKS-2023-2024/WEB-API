using Application.Comments.Commands.Create;
using Application.Labels.Commands.Delete;
using Application.Labels.Commands.Queries.FindRepositoryDefaultLabels;
using Application.Labels.Commands.Update;
using Application.Labels.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Comments.Dtos;
using WEB_API.Labels.Dtos;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;

namespace WEB_API.Labels;

[Route("labels")]
[ApiController]
public class LabelController: ControllerBase
{
    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;
    
    public LabelController(ISender sender,ITokenHandler tokenHandler,IUserIdentityService userIdentityService)
    {
        _sender = sender;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }
    
    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateLabelDto labelDto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var createdLabelId = await _sender.Send(new CreateLabelCommand(creatorId, Guid.Parse(labelDto.RepositoryId),
            labelDto.Title, labelDto.Color, labelDto.IsDefaultLabel, labelDto.Description));
        return Ok(new {Id = createdLabelId});
    }

    [HttpPost("update")]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateLabelDto labelDto)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var updatedLabel = await _sender.Send(new UpdateLabelCommand(Guid.Parse(labelDto.Id),
            labelDto.Title, labelDto.Description, labelDto.Color));
        return Ok(updatedLabel);
    }

    [HttpGet("{repositoryId}/defaults")]
    [Authorize]
    public async Task<IActionResult> FindRepositoryDefaultLabels(string repositoryId, [FromQuery] string? search)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var labels = await _sender.Send(new FindRepositoryDefaultLabelsQuery(Guid.Parse(repositoryId), search is null ? "" : search));
        return Ok(labels);
    }

    [HttpDelete("{labelId}")]
    [Authorize]
    public async Task<IActionResult> Delete(string labelId)
    {
        var deletedLabel = await _sender.Send(new DeleteLabelCommand(Guid.Parse(labelId)));
        return Ok(new { Id = deletedLabel });
    }

}