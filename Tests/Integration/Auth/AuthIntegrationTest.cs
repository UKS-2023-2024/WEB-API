using Application.Auth.Commands.Register;
using Domain.Exceptions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Tests.Integration.Setup;
using WEB_API.Auth;
using WEB_API.Auth.Dtos;

namespace Tests.Integration.Auth;

public class AuthIntegrationTest: BaseIntegrationTest
{
    public AuthIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }

    [Fact]
    async Task Register_ShouldAddUser_WhenCommandIsValid()
    {
        //Arrange
        var command = new RegisterUserCommand("sarasinjeri@gmail.com", "sarasinjeriljubavsrkijeva", "sara1404",
            "Sara Sinjeri");
        //Act
        var guid = await _sender.Send(command);
        
        //Assert 
        var user = await _context.Users.FindAsync(guid);
        user.ShouldNotBeNull();
    }

    [Fact]
    async Task Register_ShouldThrowException_WhenEmailIsNotUnique()
    {
        //Arrange
        var command = new RegisterUserCommand("test@gmail.com", "asdasdasd", "sara", "sara sinjeri");
        
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<UserWithThisEmailExistsException>(() => handle());;
    }
    
    [Fact]
    async Task Register_ShouldThrowException_WhenUsernameIsNull()
    {
        //Arrange
        var command = new RegisterUserCommand("sarasinjeri@gmail.com", "asdasdasd", null, "sara sinjeri");
        
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<Exception>(() => handle());;
    }
    
}