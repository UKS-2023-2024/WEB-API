
using System.Security.Claims;
using Application.Auth.Commands.Delete;
using Application.Auth.Commands.Register;
using Application.Auth.Queries.FindAll;
using Application.Auth.Queries.FindById;
using Application.Auth.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Auth.Dtos;
using WEB_API.Shared.TokenHandler;

namespace WEB_API.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController: ControllerBase
{

    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    
    public AuthController(ISender mediator,ITokenHandler tokenHandler)
    {
        _sender = mediator;
        _tokenHandler = tokenHandler;
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
    [Route("/users/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteUserCommand(id));
        return Ok();
    }
    
    [HttpGet]
    [Authorize]
    [Route("/current")]
    public async Task<IActionResult> GetCurrentUser()
    {
        if (HttpContext.User.Identity is not ClaimsIdentity identity) return Unauthorized();
        
        var userClaims = identity.Claims;
        IEnumerable<Claim> enumerable = userClaims as Claim[] ?? userClaims.ToArray();
        var idString = enumerable.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if (idString == null)
            return Unauthorized();
        
        var id = Guid.Parse(idString);
        var result = await _sender.Send(new FindUserByIdQuery(id));
        return Ok(new CurrentUserDto(result));
    }
    
    [HttpPost]
    [Route("/login")]
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
}