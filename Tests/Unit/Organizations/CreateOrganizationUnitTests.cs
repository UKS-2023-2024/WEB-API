using Application.Organizations.Commands.Create;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class CreateOrganizationUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IOrganizationRepository> _organizationRepository;

    public CreateOrganizationUnitTests()
    {
        _userRepositoryMock = new();
        _organizationRepository = new();
    }

    [Fact]
    public async void CreateOrganization_ShouldBeSuccess_WhenOrganizationNameUnique()
    {
        //Arrange
        var command = new CreateOrganizationCommand("my organization", "org@example.com", new List<Guid>(),
            Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"));
        User foundUser = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER);
        Organization organization = Organization.Create("my organization", "org@example.com", new List<User>());
        _userRepositoryMock.Setup(x => x.FindUserById(It.IsAny<Guid>()))
            .ReturnsAsync(foundUser);
        _organizationRepository.Setup(x => x.Create(It.IsAny<Organization>()))
            .ReturnsAsync(organization);
        var handler = new CreateOrganizationCommandHandler(_userRepositoryMock.Object, _organizationRepository.Object);
        //Act
        Guid organizationId = await handler.Handle(command, default);
        
        //Assert
        organizationId.ShouldBeOfType<Guid>();
    }
    
    [Fact]
    public async void CreateOrganization_ShouldThrowException_WhenOrganizationNameIsNotUnique()
    {
        //Arrange
        var command = new CreateOrganizationCommand("my organization", "org@example.com", new List<Guid>(),
            Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"));
        User foundUser = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER);
        Organization organization = Organization.Create("my organization", "org@example.com", new List<User>());
        Organization foundOrganization = Organization.Create("my organization", "org2@example.com", new List<User>());

        _userRepositoryMock.Setup(x => x.FindUserById(It.IsAny<Guid>()))
            .ReturnsAsync(foundUser);
        _organizationRepository.Setup(x => x.Create(It.IsAny<Organization>()))
            .ReturnsAsync(organization);
        _organizationRepository.Setup(x => x.FindByName(It.IsAny<string>()))
            .ReturnsAsync(foundOrganization);
        var handler = new CreateOrganizationCommandHandler(_userRepositoryMock.Object, _organizationRepository.Object);
        
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<OrganizationWithThisNameExistsException>(() => handle());
    }
}