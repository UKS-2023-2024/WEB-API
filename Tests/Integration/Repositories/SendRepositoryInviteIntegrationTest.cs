using Application.Organizations.Commands.SendInvite;
using Application.Repositories.Commands.StarringRepository.UnstarRepository;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class SendRepositoryInviteIntegrationTest : BaseIntegrationTest

{
    public SendRepositoryInviteIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    // [Fact]
    // public async Task Handle_SendInvite_ShouldReturnError_UserAlreadyAMember()
    // {
    //     //Arrange
    //     var command = new SendInviteCommand(user3, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"));
    //
    //     //Act
    //     async Task Handle() => await _sender.Send(command);
    //
    //     //Assert
    //     await Should.ThrowAsync<AlreadyRepositoryMemberException>(Handle);
    // }
}