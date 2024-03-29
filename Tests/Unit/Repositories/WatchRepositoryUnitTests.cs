﻿using Application.Repositories.Commands.StarringRepository.StarRepository;
using Application.Repositories.Commands.StarringRepository.UnstarRepository;
using Application.Repositories.Commands.WatchingRepository.WatchRepository;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Repositories;
using Domain.Repositories.Enums;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using FluentResults;
using Moq;
using Shouldly;

namespace Tests.Unit.Repositories;

public class WatchRepositoryUnitTests
{
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepositoryMock;
    private readonly Mock<IRepositoryWatcherRepository> _repositoryWatcherRepositoryMock;
    private readonly User _user;
    
    public WatchRepositoryUnitTests()
    {
        _user = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "email@gmail.com", "full name", "username", "password", UserRole.USER);
        var repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null, _user);
        var repository2 = Repository.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", true, null, _user);
        var repository3 = Repository.Create(new Guid("8e9b1cc3-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", true, null, _user);
        var repository4 = Repository.Create(new Guid("8e9b1cc4-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null, _user);
        repository3.AddMember(_user);
        repository4.AddToWatchedBy(_user, WatchingPreferences.AllActivity);
        RepositoryWatcher watcher = RepositoryWatcher.Create(_user.Id, WatchingPreferences.AllActivity, repository4.Id);
        _repositoryRepositoryMock = new();
        _repositoryMemberRepositoryMock = new();
        _repositoryWatcherRepositoryMock = new();
        _repositoryRepositoryMock.Setup(x => x.Find(repository1.Id)).Returns(repository1);
        _repositoryRepositoryMock.Setup(x => x.Find(repository2.Id)).Returns(repository2);
        _repositoryRepositoryMock.Setup(x => x.Find(repository3.Id)).Returns(repository3);
        _repositoryRepositoryMock.Setup(x => x.Find(repository4.Id)).Returns(repository4);
        var repositoryMember = RepositoryMember.Create(_user, repository3, RepositoryMemberRole.WRITE);
        _repositoryMemberRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user.Id, repository3.Id)).ReturnsAsync(repositoryMember);
        _repositoryWatcherRepositoryMock.Setup(x => x.FindByUserIdAndRepositoryId(_user.Id, repository4.Id)).ReturnsAsync(watcher);
    }
    
   
    
    [Fact]
    public void Watch_ShouldReturnSuccess_WhenRepositoryPublicAndUserIsNotWatching()
    {
        //Arrange
        var command = new WatchRepositoryCommand(_user,
            new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), WatchingPreferences.AllActivity);
        
        //Act
        var result = new WatchRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object, _repositoryWatcherRepositoryMock.Object).Handle(command,default);
        
        //Assert
        result.IsFaulted.ShouldBe(false);
    }

    
    [Fact]
    public async void Watch_ShouldReturnError_WhenRepositoryPrivateAndUserNotMember()
    {
        //Arrange
        var command = new WatchRepositoryCommand(_user,
            new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"), WatchingPreferences.AllActivity);
        
        var handler = new WatchRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object, _repositoryWatcherRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryInaccessibleException>(Handle);
    }
    
    [Fact]
    public async void Watch_ShouldReturnError_WhenRepositoryNotFound()
    {
        //Arrange
        var command = new WatchRepositoryCommand(_user,
            new Guid("8e9b1cc5-ffaa-4bf2-9f2c-5e00a21d92a9"), WatchingPreferences.AllActivity);
        
        var handler = new WatchRepositoryCommandHandler(_repositoryRepositoryMock.Object, _repositoryMemberRepositoryMock.Object, _repositoryWatcherRepositoryMock.Object);
        
        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
}