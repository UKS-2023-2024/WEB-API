
using Application.Auth.Commands.Delete;
using Application.Auth.Commands.Register;
using Application.Auth.Queries.FindAll;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Auth.Dtos;

namespace WEB_API.Auth;

[ApiController]
[Route("auth")]
public class AuthController: ControllerBase
{

    private ISender _sender;
    
    public AuthController(ISender mediator)
    {
        _sender = mediator;
    }

    [HttpGet]
    [Route("/users")]
    public IActionResult FindAllUsers()
    {
        var users = _sender.Send(new FindAllUsersQuery());
        return Ok(users.Result);
    }

    [HttpDelete]
    [Route("/users/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteUserCommand(id));
        return Ok();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto data)
    {
        await _sender.Send(new RegisterUserCommand(data.PrimaryEmail, data.Password, data.Username, data.Fullname));
        return Ok();
    }
}