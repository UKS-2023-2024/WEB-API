using System.Reflection;
using Application.Repositories.Commands.Fork;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Repositories;

public class ForkRepositoryUnitTests
{
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IGitService> _gitService = new();
    private readonly Mock<IRepositoryForkRepository> _repositoryForkRepository = new();
    private readonly Mock<IRepositoryRepository> _repositoryRepository = new();
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly Repository _repository1;
    private readonly Repository _repository2;
    private readonly RepositoryMember _repoMember1;
    private readonly RepositoryMember _repoMember2;
    private readonly RepositoryMember _repoMember3;
    
    public ForkRepositoryUnitTests()
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user3 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"), "dusan.janosevicehh@gmail.com", "full name", "username3", "password", UserRole.USER);
        _repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository1", "test", true, null,_user1);
        _repository2 = Repository.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository2", "test", false, null,_user1);
        _repoMember1 = RepositoryMember.Create(_user1, _repository1, RepositoryMemberRole.OWNER);
        OverrideId(_repoMember1, new Guid("8e9b1111-ffaa-4bf2-9f2c-5e00a21d92a9"));
        _repoMember2 = RepositoryMember.Create(_user2, _repository1, RepositoryMemberRole.WRITE);
        OverrideId(_repoMember2, new Guid("8e9b3333-ffaa-4bf2-9f2c-5e00a21d92a9"));
        _repoMember3 = RepositoryMember.Create(_user1, _repository2, RepositoryMemberRole.OWNER);
        OverrideId(_repoMember3, new Guid("8e9b1188-ffaa-4bf2-9f2c-5e00a21d92a9"));
        _repository1.AddBranch(Branch.Create("grana1",_repository1.Id,true,_user1.Id));
        _repository1.AddBranch(Branch.Create("grana2",_repository1.Id,false,_user1.Id));
        _repository1.AddBranch(Branch.Create("grana3",_repository1.Id,false,_user1.Id));
        _repository2.AddBranch(Branch.Create("grana1",_repository2.Id,true,_user1.Id));
        
        OverrideMemberList(_repository1, new List<RepositoryMember>{_repoMember1,_repoMember2});
        OverrideMemberList(_repository2, new List<RepositoryMember>{_repoMember3});

        _repositoryRepository.Setup(x => x.Find(_repository1.Id)).Returns(_repository1);
        _repositoryRepository.Setup(x => x.Find(_repository2.Id)).Returns(_repository2);
        _repositoryRepository.Setup(x => x.FindByNameAndOwnerId(_repository1.Name,_user1.Id)).ReturnsAsync(_repository1);
        _repositoryRepository.Setup(x => x.FindByNameAndOwnerId(_repository2.Name,_user1.Id)).ReturnsAsync(_repository2);

        _userRepository.Setup(x => x.FindUserById(_user1.Id)).ReturnsAsync(_user1);
        _userRepository.Setup(x => x.FindUserById(_user2.Id)).ReturnsAsync(_user2);
        _userRepository.Setup(x => x.FindUserById(_user3.Id)).ReturnsAsync(_user3);
        
        _repositoryMemberRepository.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository1.Id)).ReturnsAsync(_repoMember1);
        _repositoryMemberRepository.Setup(x => x.FindByUserIdAndRepositoryId(_user2.Id,_repository1.Id)).ReturnsAsync(_repoMember2);
        _repositoryMemberRepository.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository2.Id)).ReturnsAsync(_repoMember3);

        var dummyRepo = Repository.Create(new Guid(), "repository1", "test", true,
            null, _user1);
        _repositoryRepository.Setup(x => x.Create(It.IsAny<Repository>())).ReturnsAsync(dummyRepo);
    }
        
        private T OverrideId<T>(T obj, Guid id)
        {
            var propertyInfo = typeof(T).GetProperty("Id");
            if (propertyInfo == null) return obj;
            propertyInfo.SetValue(obj, id);
            return obj;
        }
    
        private Repository OverrideMemberList(Repository repo, List<RepositoryMember> members)
        {
            var propertyInfo = typeof(Repository).GetField("_members", BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo == null) return repo;
            propertyInfo.SetValue(repo, members);
            return repo;
        }
        
        [Fact]
        public void Handle_ShouldReturnSuccess_WhenUserMemberAndRepositoryPrivate()
        {
            //Arrange
            var command = new ForkRepositoryCommand(_user2.Id,
                _repository1.Id);

            //Act
            var result = new ForkRepositoryCommandHandler(_repositoryRepository.Object,
                    _repositoryMemberRepository.Object, _userRepository.Object,_gitService.Object,_repositoryForkRepository.Object)
                .Handle(command,default);

            //Assert
            result.IsFaulted.ShouldBe(false);
        }
        
        [Fact]
        public void Handle_ShouldReturnSuccess_WhenUserMemberAndRepositoryPublic()
        {
            //Arrange
            var command = new ForkRepositoryCommand(_user2.Id,
                _repository2.Id);

            //Act
            var result = new ForkRepositoryCommandHandler(_repositoryRepository.Object,
                    _repositoryMemberRepository.Object, _userRepository.Object,_gitService.Object,_repositoryForkRepository.Object)
                .Handle(command,default);

            //Assert
            result.IsFaulted.ShouldBe(false);
        }
        
        [Fact]
        public async void Handle_ShouldReturnError_WhenUserNotFound()
        {
            //Arrange
            var command = new ForkRepositoryCommand(new Guid(),
                _repository2.Id);
            
            var handler = new ForkRepositoryCommandHandler(_repositoryRepository.Object,
                _repositoryMemberRepository.Object, _userRepository.Object,_gitService.Object,_repositoryForkRepository.Object);

            //Act
            async Task Handle() => await handler.Handle(command, default);

            //Assert
            await Should.ThrowAsync<UserNotFoundException>(Handle);
        }
        
        [Fact]
        public async void Handle_ShouldReturnError_WhenRepositoryPrivateAndUserNotMember()
        {
            //Arrange
            var command = new ForkRepositoryCommand(_user3.Id,
                _repository1.Id);
            
            var handler = new ForkRepositoryCommandHandler(_repositoryRepository.Object,
                    _repositoryMemberRepository.Object, _userRepository.Object,_gitService.Object,_repositoryForkRepository.Object);

            //Act
            async Task Handle() => await handler.Handle(command, default);

            //Assert
            await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
        }
        
        [Fact]
        public async void Handle_ShouldReturnError_WhenRepositoryToForkNotFound()
        {
            //Arrange
            var command = new ForkRepositoryCommand(_user2.Id,
                new Guid());
            
            var handler = new ForkRepositoryCommandHandler(_repositoryRepository.Object,
                _repositoryMemberRepository.Object, _userRepository.Object,_gitService.Object,_repositoryForkRepository.Object);

            //Act
            async Task Handle() => await handler.Handle(command, default);

            //Assert
            await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
        }
        
        [Fact]
        public async void Handle_ShouldReturnError_WhenUserAlreadyHasRepositoryWithForkRepositoryName()
        {
            //Arrange
            var command = new ForkRepositoryCommand(_user1.Id,
                _repository2.Id);
            
            var handler = new ForkRepositoryCommandHandler(_repositoryRepository.Object,
                _repositoryMemberRepository.Object, _userRepository.Object,_gitService.Object,_repositoryForkRepository.Object);

            //Act
            async Task Handle() => await handler.Handle(command, default);

            //Assert
            await Should.ThrowAsync<YouAlreadyHaveRepositoryWithThisNameException>(Handle);
        }
}