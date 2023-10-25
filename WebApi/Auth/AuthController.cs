using Application.Auth.Commands.Create;
using Application.Auth.Commands.Delete;
using Application.Auth.Queries.FindAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Auth.Dtos;

namespace WEB_API.Auth;

[ApiController]
[Route("auth")]
public class AuthController: ControllerBase
{

    private IMediator _mediator;
    
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("/users")]
    public IActionResult FindAllUsers()
    {
        var users = _mediator.Send(new FindAllUsersQuery());
        return Ok(users.Result);
    }


    [HttpPost]
    [Route("/users")]
    public IActionResult Create([FromBody] CreateUserDto data)
    {
        var result = _mediator.Send(new CreateUserCommand(data.Email, data.FullName));
        if (result.IsCompleted) return Ok();
        return BadRequest(result.Exception);
    }

    [HttpDelete]
    [Route("/users/{id}")]
    public IActionResult Delete(Guid id)
    {
        var result = _mediator.Send(new DeleteUserCommand(id));
        if (result.IsCompleted) return Ok();
        return BadRequest(result.Exception);
    }
}