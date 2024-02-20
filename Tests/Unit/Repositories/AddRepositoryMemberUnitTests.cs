using System.Reflection;
using Application.Repositories.Commands.HandleRepositoryMembers.AddRepositoryMember;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Organizations.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Repositories;

public class AddRepositoryMemberUnitTests
{
    private readonly Mock<IRepositoryRepository> _repositoryRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IRepositoryInviteRepository> _repositoryInviteRepository = new();
    private readonly Mock<IGitService> _gitServiceMock = new();
    private readonly User _user1;
    private readonly User _user2;
    private readonly Repository _repository1;
    
    
    public AddRepositoryMemberUnitTests()
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        
        _repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null, _user1);
        _userRepository.Setup(x => x.FindUserById(_user1.Id)).ReturnsAsync(_user1);
        _userRepository.Setup(x => x.FindUserById(_user2.Id)).ReturnsAsync(_user2);
        _repositoryRepository.Setup(x => x.Find(_repository1.Id)).Returns(_repository1);
    }
    
    private RepositoryInvite OverrideDate(RepositoryInvite invite, DateTime date)
    {
        var propertyInfo = typeof(RepositoryInvite).GetProperty("ExpiresAt");
        if (propertyInfo == null) return invite;
        propertyInfo.SetValue(invite, date);
        return invite;
    }
    
    private RepositoryInvite OverrideRepositoryInviteId(RepositoryInvite invite, Guid id)
    {
        var propertyInfo = typeof(RepositoryInvite).GetProperty("Id");
        if (propertyInfo == null) return invite;
        propertyInfo.SetValue(invite, id);
        return invite;
    }
    
    [Fact]
    public void Handle_ShouldReturnSuccess_WhenInviteNotExpired()
    {
        //Arrange
        var repositoryInvite = RepositoryInvite.Create(_user1.Id, _repository1.Id);
        OverrideRepositoryInviteId(repositoryInvite,new Guid("aaaa1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        _repositoryInviteRepository.Setup(x => x.Find(repositoryInvite.Id)).Returns(repositoryInvite);
        
        var command = new AddRepositoryMemberCommand(repositoryInvite.Id);

        //Act
        var result = new AddRepositoryMemberCommandHandler(_userRepository.Object,_repositoryInviteRepository.Object,
            _repositoryRepository.Object, _gitServiceMock.Object).Handle(command,default);

        //Assert
        result.IsFaulted.ShouldBe(false);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenInviteExpired()
    {
        //Arrange
        var repositoryInvite = RepositoryInvite.Create(_user1.Id, _repository1.Id);
        OverrideRepositoryInviteId(repositoryInvite,new Guid("aaaa1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        OverrideDate(repositoryInvite,DateTime.Now.AddDays(-2));
        _repositoryInviteRepository.Setup(x => x.Find(repositoryInvite.Id)).Returns(repositoryInvite);
        
        var command = new AddRepositoryMemberCommand(repositoryInvite.Id);

        var handler = new AddRepositoryMemberCommandHandler(_userRepository.Object,_repositoryInviteRepository.Object,
            _repositoryRepository.Object, _gitServiceMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<InvitationExpiredException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryNotFound()
    {
        //Arrange
        var repositoryInvite = RepositoryInvite.Create(_user1.Id, new Guid("acda1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        OverrideRepositoryInviteId(repositoryInvite,new Guid("aaaa1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        _repositoryInviteRepository.Setup(x => x.Find(repositoryInvite.Id)).Returns(repositoryInvite);
        
        var command = new AddRepositoryMemberCommand(repositoryInvite.Id);

        var handler = new AddRepositoryMemberCommandHandler(_userRepository.Object,_repositoryInviteRepository.Object,
            _repositoryRepository.Object, _gitServiceMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserNotFound()
    {
        //Arrange
        var repositoryInvite = RepositoryInvite.Create(new Guid("acda1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), _repository1.Id);
        OverrideRepositoryInviteId(repositoryInvite,new Guid("aaaa1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        _repositoryInviteRepository.Setup(x => x.Find(repositoryInvite.Id)).Returns(repositoryInvite);
        
        var command = new AddRepositoryMemberCommand(repositoryInvite.Id);

        var handler = new AddRepositoryMemberCommandHandler(_userRepository.Object,_repositoryInviteRepository.Object,
            _repositoryRepository.Object, _gitServiceMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<UserNotFoundException>(Handle);
    }
    
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenInvitationNotFound()
    {
        //Arrange
        var repositoryInvite = RepositoryInvite.Create(_user1.Id, _repository1.Id);
        OverrideRepositoryInviteId(repositoryInvite,new Guid("aaaa1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        _repositoryInviteRepository.Setup(x => x.Find(repositoryInvite.Id)).Returns(repositoryInvite);
        
        var command = new AddRepositoryMemberCommand(new Guid("bbbb1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));

        var handler = new AddRepositoryMemberCommandHandler(_userRepository.Object,_repositoryInviteRepository.Object,
            _repositoryRepository.Object, _gitServiceMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryInviteNotFound>(Handle);
    }
}