
using System.Security.Claims;
using Application.Auth.Commands.Delete;
using Application.Auth.Commands.Register;
using Application.Auth.Commands.Update;
using Application.Auth.Queries.FindAll;
using Application.Auth.Queries.FindById;
using Application.Auth.Queries.Login;
using Domain.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Auth.Dtos;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;

namespace WEB_API.Auth;

[ApiController]
[Route("auth")]
public class AuthController: ControllerBase
{

    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;
    
    public AuthController(ISender mediator,ITokenHandler tokenHandler,IUserIdentityService userIdentityService)
    {
        _sender = mediator;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    [Route("/users")]
    public IActionResult FindAllUsers()
    {
        var users = _sender.Send(new FindAllUsersQuery());
        return Ok(users.Result);
    }

    [HttpDelete]
    [Authorize]
    [Route("/user")]
    public async Task<IActionResult> Delete()
    {
        var id = _userIdentityService.FindUserIdentity(HttpContext.User);
        
        await _sender.Send(new DeleteUserCommand(id));
        return Ok();
    }
    
    [HttpGet]
    [Authorize]
    [Route("current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var id = _userIdentityService.FindUserIdentity(HttpContext.User);
        
        var result = await _sender.Send(new FindUserByIdQuery(id));
        return Ok(new CurrentUserDto(result));
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto data)
    {
        var user = await _sender.Send(new LoginQuery(data.Email, data.Password));
        var jwtToken = _tokenHandler.Generate(user);
        return Ok(jwtToken);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto data)
    {
        await _sender.Send(new RegisterUserCommand(data.PrimaryEmail, data.Password, data.Username, data.Fullname));
        return Ok();
    }

    [HttpPatch]
    [Authorize]
    [Route("/user")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto data)
    {
        var id = _userIdentityService.FindUserIdentity(HttpContext.User);
        List<SocialAccount> acc = SocialAccountDto.SocialAccountsFromSocialAccountDtos(data.SocialAccounts, id);
        await _sender.Send(new UpdateUserCommand(id, data.FullName, data.Bio, data.Company, data.Location, data.Website, acc));
        return Ok();
    }
    
}