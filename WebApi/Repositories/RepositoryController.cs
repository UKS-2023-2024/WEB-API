using Application.Auth.Commands.Update;
using Application.Repositories.Commands.Create.CreateForOrganization;
using Application.Repositories.Commands.Create.CreateForUser;
using Application.Repositories.Commands.Delete;
using Application.Repositories.Commands.Fork;
using Application.Repositories.Commands.HandleRepositoryMembers.AddRepositoryMember;
using Application.Repositories.Commands.HandleRepositoryMembers.ChangeRole;
using Application.Repositories.Commands.HandleRepositoryMembers.RemoveRepositoryMember;
using Application.Repositories.Commands.HandleRepositoryMembers.SendRepositoryInvite;
using Application.Repositories.Commands.StarringRepository.StarRepository;
using Application.Repositories.Commands.StarringRepository.UnstarRepository;
using Application.Repositories.Commands.WatchingRepository.UnwatchRepository;
using Application.Repositories.Commands.WatchingRepository.WatchRepository;
using Application.Repositories.Queries.DidUserStarRepository;
using Application.Repositories.Queries.FindAllByOrganizationId;
using Application.Repositories.Queries.FindAllByOwnerId;
using Application.Repositories.Queries.FindAllRepositoriesUserBelongsTo;
using Application.Repositories.Queries.FindAllRepositoryMembers;
using Application.Repositories.Queries.FindAllUsersThatStarredRepository;
using Application.Repositories.Queries.FindAllUsersWatchingRepository;
using Application.Repositories.Queries.FindNumberOfForks;
using Application.Repositories.Queries.FindRepositoryMemberRole;
using Application.Repositories.Queries.IsUserWatchingRepository;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Auth.Presenter;
using WEB_API.Repositories.Dtos;
using WEB_API.Repositories.Presenters;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;

namespace WEB_API.Repositories;


[ApiController]
[Route("repositories")]
public class RepositoryController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;

    public RepositoryController(ISender sender,ITokenHandler tokenHandler,IUserIdentityService userIdentityService)
    {
        _sender = sender;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }

    [HttpPost("user")]
    [Authorize]
    public async Task<IActionResult> CreateForUser([FromBody] UserRepositoryDto repositoryDto)
    {
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);

        var createdRepositoryId = await _sender.Send(new CreateRepositoryForUserCommand(repositoryDto.Name, repositoryDto.Description, 
            repositoryDto.IsPrivate, creatorId));

        return Ok(createdRepositoryId);
    }


    [HttpPost("organization")]
    [Authorize]
    public async Task<IActionResult> CreateForOrganization([FromBody] OrganizationRepositoryDto dto)
    {
        var creatorId = _userIdentityService.FindUserIdentity(HttpContext.User);

        var createdRepositoryId = await _sender.Send(new CreateRepositoryForOrganizationCommand(dto.Name, dto.Description,
            dto.IsPrivate, creatorId, dto.OrganizationId));

        return Ok(createdRepositoryId);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);

        await _sender.Send(new DeleteRepositoryCommand(userId, Guid.Parse(id)));

        return Ok();
    }


    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UpdateRepositoryDto dto)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        Repository repository = await _sender.Send(new UpdateRepositoryCommand(userId, dto.Id, dto.Name, dto.Description, dto.IsPrivate));

        return Ok(repository);
    }

    [HttpGet("owner")]
    [Authorize]
    public async Task<IActionResult> FindLoggedUserRepositories()
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var repositories = await _sender.Send(new FindAllRepositoriesByOwnerIdQuery(userId));
        return Ok(RepositoryPresenter.MapRepositoriesToPresenters(repositories));
    }
    
    [HttpGet("did-user-star/{repositoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> DidUserStarRepository(Guid repositoryId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var didUserStar = await _sender.Send(new DidUserStarRepositoryQuery(userId,repositoryId));
        return Ok(didUserStar);
    }
    
    [HttpGet("forks/{repositoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetNumberOfForks(Guid repositoryId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var numberOfForks = await _sender.Send(new FindNumberOfForksCommand(userId,repositoryId));
        return Ok(numberOfForks);
    }
    
    [HttpGet("users-that-starred/{repositoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetAllThatStarred(Guid repositoryId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var usersThatStarred = await _sender.Send(new FindAllUsersThatStarredQuery(userId,repositoryId));
        return Ok(UserThatStarredPresenter.MapUserToStarredPresenters(usersThatStarred));
    }
    
    [HttpPatch("star/{repositoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> StarRepository(Guid repositoryId)
    {
        var user = await _userIdentityService.FindUserFromToken(HttpContext.User);
        if (user is null)
            return Unauthorized();
        await _sender.Send(new StarRepositoryCommand(user,repositoryId));
        return Ok();
    }
    
    [HttpPatch("unstar/{repositoryId}")]
    [Authorize]
    public async Task<IActionResult> UnstarRepository(Guid repositoryId)
    {
        var user = await _userIdentityService.FindUserFromToken(HttpContext.User);
        if (user is null)
            return Unauthorized();
        await _sender.Send(new UnstarRepositoryCommand(user,repositoryId));
        return Ok();
    }

    [HttpPost("send-invite/{repositoryId:guid}/{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> SendRepositoryInvite(Guid repositoryId, Guid userId)
    {
        var ownerId = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new SendInviteCommand(ownerId, userId,repositoryId));
        return Ok();
    }
    
    [HttpPost("forks/{repositoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> ForkRepository(Guid repositoryId)
    {
        var ownerId = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new ForkRepositoryCommand(ownerId,repositoryId));
        return Ok();
    }
    
    
    [HttpGet("{repositoryId:guid}/member-role")]
    [Authorize]
    public async Task<IActionResult> GetUserRole(Guid repositoryId)
    {
        var user = await _userIdentityService.FindUserFromToken(HttpContext.User);
        if (user is null)
            return Unauthorized();
        var role = await _sender.Send(new FindRepositoryMemberRoleQuery(user.Id,repositoryId));
        return Ok(role);
    }
    
    [HttpPost("add-user/{inviteId:guid}")]
    public async Task<IActionResult> AddUserToRepository(Guid inviteId)
    {
        await _sender.Send(new AddRepositoryMemberCommand(inviteId));
        return Ok();
    }
    
    [HttpDelete("remove-user/{repositoryId:guid}/{repositoryMemberId:guid}")]
    [Authorize]
    public async Task<IActionResult> RemoveUserFromRepository(Guid repositoryId, Guid repositoryMemberId)
    {
        var ownerId = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new RemoveRepositoryMemberCommand(ownerId, repositoryMemberId,repositoryId));
        return Ok();
    }
    
    [HttpGet("{repositoryId:guid}/members")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<RepositoryMemberPresenter>>> GetRepositoryMembers(Guid repositoryId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var members =  await _sender.Send(new FindAllRepositoryMembersQuery(userId, repositoryId));
        return Ok(RepositoryMemberPresenter.MapRepositoryMembersToPresenters(members));
    }
    
    [HttpPatch("change-user-role/{repositoryId:guid}/{repositoryMemberId:guid}/{role}")]
    [Authorize]
    public async Task<IActionResult> ChangeRepositoryMemberRole(Guid repositoryId, Guid repositoryMemberId,RepositoryMemberRole role)
    {
        var ownerId = _userIdentityService.FindUserIdentity(HttpContext.User);
        await _sender.Send(new ChangeMemberRoleCommand(ownerId, repositoryMemberId, repositoryId, role));
        return Ok();
    }
    

    [HttpGet("organization/{organizationId}")]
    [Authorize]
    public async Task<IActionResult> FindAllByOrganizationIdAsync(Guid organizationId)
    {
        var repositories = await _sender.Send(new FindAllRepositoriesByOrganizationIdQuery(organizationId));
        return Ok(RepositoryPresenter.MapRepositoriesToPresenters(repositories));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> FindRepositoriesUserBelongsTo()
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var repositories = await _sender.Send(new FindAllRepositoriesUserBelongsToQuery(userId));
        return Ok(RepositoryPresenter.MapRepositoriesToPresenters(repositories));
    }


    [HttpPatch("watch")]
    [Authorize]
    public async Task<IActionResult> WatchRepository([FromBody] WatchRepositoryDto dto)
    {
        var user = await _userIdentityService.FindUserFromToken(HttpContext.User);
        if (user is null)
            return Unauthorized();
        await _sender.Send(new WatchRepositoryCommand(user, dto.RepositoryId, dto.WatchingPreferences));
        return Ok();
    }


    [HttpGet("is-user-watching/{repositoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> IsUserWatchingRepository(Guid repositoryId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var watchingPref = await _sender.Send(new IsUserWatchingRepositoryQuery(userId, repositoryId));
        return Ok(watchingPref);
    }

    [HttpPatch("unwatch/{repositoryId}")]
    [Authorize]
    public async Task<IActionResult> UnwatchRepository(Guid repositoryId)
    {
        var user = await _userIdentityService.FindUserFromToken(HttpContext.User);
        if (user is null)
            return Unauthorized();
        await _sender.Send(new UnwatchRepositoryCommand(user, repositoryId));
        return Ok();
    }

    [HttpGet("users-watching/{repositoryId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetAllWatching(Guid repositoryId)
    {
        var userId = _userIdentityService.FindUserIdentity(HttpContext.User);
        var watchers = await _sender.Send(new FindAllUsersWatchingQuery(userId, repositoryId));
        return Ok(RepositoryWatcherPresenter.MapRepositoryWatcherToRepositoryWatcherPresenters(watchers));
    }
}