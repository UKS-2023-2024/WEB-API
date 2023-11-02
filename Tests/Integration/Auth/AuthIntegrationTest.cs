using Application.Auth.Commands.Delete;
using Application.Auth.Commands.Register;
using Application.Auth.Commands.Update;
using Application.Auth.Queries.Login;
using Domain.Auth;
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
    
    [Fact]
    async Task Login_ShouldThrowException_WhenEmailNotValid()
    {
        //Arrange
        var command = new LoginQuery("test123@gmail.com", "asdasdasdasd");
        
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<Exception>(() => handle());;
    }
    
    [Fact]
    async Task Login_ShouldThrowException_WhenPasswordNotValid()
    {
        //Arrange
        var command = new LoginQuery("test@gmail.com", "123123123");
        
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<Exception>(() => handle());;
    }
    
    [Fact]
    async Task Login_ShouldReturnUser_WhenQueryIsValid()
    {
        //Arrange
        var command = new LoginQuery("test@gmail.com", "asdasdasdasd");
        
        //Act
        var user = await _sender.Send(command);

        //Assert
        user.ShouldNotBeNull();
    }


    [Fact]  
    async Task Update_ShouldUpdateUser_WhenCommandIsValid()
    {
        //Arrange
        var command = new UpdateUserCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "", "", "", "", "", new List<SocialAccount>());

        //Act
        var user = await _sender.Send(command);

        //Assert 
        user.ShouldNotBeNull();
    }

    [Fact]
    async Task Update_ShouldThrowException_WhenIdIsInvalid()
    {
        //Arrange
        var command = new UpdateUserCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a1"), "", "", "", "", "", new List<SocialAccount>());

        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<UserNotFoundException>(() => handle());
    }

    [Fact]
    async Task Delete_ShouldDeleteUser_WhenCommandIsValid()
    {
        //Arrange
        var command = new DeleteUserCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        var user = await _sender.Send(command);

        //Assert 
        user.ShouldNotBeNull();
    }

    [Fact]
    async Task Delete_ShouldThrowException_WhenIdIsInvalid()
    {
        //Arrange
        var command = new DeleteUserCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a1"));

        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<UserNotFoundException>(() => handle());
    }

}