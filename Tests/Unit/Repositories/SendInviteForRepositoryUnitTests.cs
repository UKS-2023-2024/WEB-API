using Application.Repositories.Commands.HandleRepositoryMembers.SendRepositoryInvite;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using MediatR;
using Moq;
using Shouldly;

namespace Tests.Unit.Repositories;

public class SendInviteForRepositoryUnitTests
{
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IRepositoryInviteRepository> _repositoryInviteRepository = new();
    private readonly Mock<IRepositoryRepository> _repositoryRepository = new();
    private readonly Mock<IOrganizationMemberRepository> _organizationMemberRepository = new();
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly Repository _repository1;
    private readonly Repository _repository2;
    private readonly Repository _repository3;

    public SendInviteForRepositoryUnitTests()
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user3 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);

        var organization = Organization.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"),"orgName", "dusanjanosevic007@gmail.com",
            new List<User>());
        
        organization.AddMember(OrganizationMember.Create(_user2.Id,organization.Id,OrganizationRole.Create("some","some")));
        organization.AddMember(OrganizationMember.Create(_user1.Id,organization.Id,OrganizationRole.Create("some","some")));
        
        _repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository1", "test", false, null);
        _repository2 = Repository.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository2", "test", false, null);
        _repository3 = Repository.Create(new Guid("8e9b1cc3-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository3", "test", false, organization);

        var repoMember1 = RepositoryMember.Create(_user1, _repository1, RepositoryMemberRole.OWNER);
        var repoMember2 = RepositoryMember.Create(_user1, _repository1, RepositoryMemberRole.OWNER);
        var repoMember3 = RepositoryMember.Create(_user2, _repository1, RepositoryMemberRole.CONTRIBUTOR);
        var repoMember4 = RepositoryMember.Create(_user1, _repository3, RepositoryMemberRole.OWNER);
        _repository1.AddMember(repoMember1);
        _repository1.AddMember(repoMember3);
        _repository2.AddMember(repoMember2);
        _repository3.AddMember(repoMember4);

        _userRepository.Setup(x => x.Find(_user1.Id)).Returns(_user1);
        _userRepository.Setup(x => x.Find(_user2.Id)).Returns(_user2);
        _userRepository.Setup(x => x.Find(_user3.Id)).Returns(_user3);
        
        _repositoryRepository.Setup(x => x.Find(_repository1.Id)).Returns(_repository1);
        _repositoryRepository.Setup(x => x.Find(_repository2.Id)).Returns(_repository2);
        _repositoryRepository.Setup(x => x.Find(_repository3.Id)).Returns(_repository3);

        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository1.Id)).ReturnsAsync(repoMember1);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository2.Id)).ReturnsAsync(repoMember2);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user2.Id,_repository2.Id)).ReturnsAsync(repoMember3);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository3.Id)).ReturnsAsync(repoMember4);
        _mediator.Setup(o =>
            o.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserAlreadyMember()
    {
        //Arrange
        var command = new SendInviteCommand(_user1.Id,
            _user2.Id,_repository2.Id);
        
        var handler = new SendInviteCommandHandler(_mediator.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryInviteRepository.Object,_userRepository.Object,_repositoryRepository.Object,_organizationMemberRepository.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<AlreadyRepositoryMemberException>(Handle);
    }
    
    [Fact]
    public void Handle_ShouldReturnSuccess_WhenUserNotAMember()
    {
        //Arrange
        var command = new SendInviteCommand(_user1.Id,
            _user2.Id,_repository1.Id);
        
        //Act
        var result = new SendInviteCommandHandler(_mediator.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryInviteRepository.Object,_userRepository.Object,_repositoryRepository.Object,_organizationMemberRepository.Object).Handle(command,default);

        //Assert
        result.IsFaulted.ShouldBe(false);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenOwnerNotAnOwner()
    {
        //Arrange
        var command = new SendInviteCommand(_user2.Id,
            _user3.Id,_repository2.Id);
        
        var handler = new SendInviteCommandHandler(_mediator.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryInviteRepository.Object,_userRepository.Object,_repositoryRepository.Object,_organizationMemberRepository.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<MemberNotOwnerException>(Handle);
    }
    [Fact]
    public async void Handle_ShouldReturnError_WhenOwnerNotExists()
    {
        //Arrange
        var command = new SendInviteCommand(new Guid("8e9b1aaa-35d3-4bf2-9f2c-5e00a21d92a8"),
            _user3.Id,_repository2.Id);
        
        var handler = new SendInviteCommandHandler(_mediator.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryInviteRepository.Object,_userRepository.Object,_repositoryRepository.Object,_organizationMemberRepository.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserToAddNotExists()
    {
        //Arrange
        var command = new SendInviteCommand(_user1.Id,
            new Guid("8e9b1aaa-35d3-4bf2-9f2c-5e00a21d92a8"),_repository2.Id);
        
        var handler = new SendInviteCommandHandler(_mediator.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryInviteRepository.Object,_userRepository.Object,_repositoryRepository.Object,_organizationMemberRepository.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<UserNotFoundException>(Handle);
    }
    
    [Fact]
    public void Handle_ShouldReturnSuccess_WhenUserNotAMemberAndInOrganization()
    {
        //Arrange
        var command = new SendInviteCommand(_user1.Id,
            _user2.Id,_repository3.Id);
        
        //Act
        var result = new SendInviteCommandHandler(_mediator.Object, _repositoryMemberRepositoryMock.Object,
            _repositoryInviteRepository.Object,_userRepository.Object,_repositoryRepository.Object,_organizationMemberRepository.Object).Handle(command,default);

        //Assert
        result.IsFaulted.ShouldBe(false);
    }
}