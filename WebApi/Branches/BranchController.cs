using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;
using WEB_API.Branches.Dtos;
using Application.Branches.Commands.Create;
using Application.Branches.Commands.Update;
using Application.Branches.Commands.Delete;
using Application.Branches.Commands.Restore;
using Application.Repositories.Queries.FindAllUserWithoutDefaultByRepositoryId;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryId;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryIdPagination;
using Application.Repositories.Queries.FindDefaultBranchByRepositoryId;


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
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var createdBranchId = await _sender.Send(new CreateBranchCommand(dto.Name, dto.RepositoryId, false, userId));
        return Ok(createdBranchId);
    }

    [HttpPatch("")]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateBranchDto dto)
    {
        var updatedBranch = await _sender.Send(new UpdateBranchNameCommand(dto.Id, dto.Name));
        return Ok(updatedBranch);
    }

    [HttpPatch("make-default/{id}")]
    [Authorize]
    public async Task<IActionResult> MakeDefault(Guid id)
    {
        var updatedBranch = await _sender.Send(new MakeBranchDefaultCommand(id));
        return Ok(updatedBranch);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletedBranch = await _sender.Send(new DeleteBranchCommand(id));
        return Ok(deletedBranch);
    }

    [HttpPatch("restore/{id}")]
    [Authorize]
    public async Task<IActionResult> Restore(Guid id)
    {
        var restoredBranch = await _sender.Send(new RestoreBranchCommand(id));
        return Ok(restoredBranch);
    }

    [HttpGet("without-default/{repositoryId}")]
    [Authorize]
    public async Task<IActionResult> FindAllWithoutDefauly(Guid repositoryId)
    {
        var branches = await _sender.Send(new FindAllBranchesWithoutDefaultByRepositoryIdQuery(repositoryId));
        return Ok(branches);
    }

    [HttpGet("without-default/{repositoryId}/{pageSize}/{pageNumber}")]
    [Authorize]
    public async Task<IActionResult> FindAllWithoutDefault(Guid repositoryId, int pageSize, int pageNumber)
    {
        var branches = await _sender.Send(new FindAllBranchesWithoutDefaultByRepositoryIdPaginationQuery(repositoryId, pageSize, pageNumber));
        return Ok(branches);
    }

    [HttpGet("user/without-default/{repositoryId}/{pageSize}/{pageNumber}")]
    [Authorize]
    public async Task<IActionResult> FindAllUserBranchesWithoutDefault(Guid repositoryId, int pageSize, int pageNumber)
    {
        var ownerId = _userIdentityService.FindUserIdentity(HttpContext.User);

        var branches = await _sender.Send(new FindAllUserBranchesWithoutDefaultByRepositoryIdQuery(repositoryId, ownerId, pageSize, pageNumber));
        return Ok(branches);
    }


    [HttpGet("default/{repositoryId}")]
    [Authorize]
    public async Task<IActionResult> FindDefault(Guid repositoryId)
    {
        var branches = await _sender.Send(new FindDefaultBranchByRepositoryIdQuery(repositoryId));
        return Ok(branches);
    }
}