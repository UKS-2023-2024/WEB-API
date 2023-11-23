using Application.Milestones.Commands.Create;
using Application.Milestones.Commands.Update;
using Application.Milestones.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Milestones.Dtos;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;
using WEB_API.Branches.Dtos;
using Application.Branches.Commands.Create;

namespace WEB_API.Branches;

[ApiController]
[Route("branches")]
public class BranchController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;

    public BranchController(ISender sender, ITokenHandler tokenHandler, IUserIdentityService userIdentityService)
    {
        _sender = sender;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }

    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] BranchDto dto)
    {
        var createdBranchId = await _sender.Send(new CreateBranchCommand(dto.Name, dto.RepositoryId, false));
        return Ok(createdBranchId);
    }
}