using Application.Comments.Commands.Create;
using Application.Comments.Commands.Delete;
using Application.Comments.Queries.GetTaskComments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Comments.Dtos;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;

namespace WEB_API.Comments;

[ApiController]
[Route("comments")]
public class CommentController: ControllerBase
{
    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;
    
    public CommentController(ISender sender,ITokenHandler tokenHandler,IUserIdentityService userIdentityService)
    {
        _sender = sender;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }

    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCommentDto commentDto)
    {
        var parentId = commentDto.ParentId is null ? (Guid?)null : Guid.Parse(commentDto.ParentId);
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var createdCommentId = await _sender.Send(new CreateCommentCommand(creatorId, Guid.Parse(commentDto.TaskId), commentDto.Content, parentId));
        return Ok(new {Id = createdCommentId});
    }
    
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Create(string id)
    {
        Guid creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var createdCommentId = await _sender.Send(new DeleteCommentCommand(Guid.Parse(id)));
        return Ok(new {Id = createdCommentId});
    }

    [HttpGet("{taskId}")]
    [Authorize]
    public async Task<IActionResult> GetTaskComments(string taskId)
    {
        var comments = await _sender.Send(new GetTaskCommentsQuery(new Guid(), Guid.Parse(taskId)));
        return Ok(comments);
    }
}