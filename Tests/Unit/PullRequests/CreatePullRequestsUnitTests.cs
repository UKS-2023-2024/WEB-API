using System.Reflection;
using Application.PullRequests.Commands;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.PullRequests;

public class CreatePullRequestsUnitTests
{
    private readonly Mock<IPullRequestRepository> _pullRequestRepository = new();
    private readonly Mock<IRepositoryMemberRepository> _repositoryMemberRepository = new();
    private readonly Mock<ITaskRepository> _taskRepository = new();
    private readonly Mock<IRepositoryRepository> _repositoryRepository = new();
    private readonly Mock<ILabelRepository> _labelRepository = new();
    private readonly Mock<INotificationService> _notificationService = new();
    private readonly Mock<IBranchRepository> _branchRepository = new();
    private readonly Mock<IGitService> _gitService = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IIssueRepository> _issueRepository = new();
    private readonly User _user1;
    private readonly User _user2;
    private readonly User _user3;
    private readonly RepositoryMember _repoMember1; 
    private readonly RepositoryMember _repoMember2; 
    private readonly Repository _repository1;
    private readonly Branch _branch1;
    private readonly Branch _branch2;
    private readonly Branch _branch3;

    public CreatePullRequestsUnitTests()
    {
        _user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        _user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        _user3 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92b9"), "dusan.janosevicehh@gmail.com", "full name", "username3", "password", UserRole.USER);

        _repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository1", "test", false, null,_user1);
        _repoMember1 = RepositoryMember.Create(_user1, _repository1, RepositoryMemberRole.OWNER);
        _repoMember1 = OverrideId(_repoMember1, new Guid("8e9b1111-ffaa-4bf2-9f2c-5e00a21d92a9"));
        _repoMember2 = RepositoryMember.Create(_user2, _repository1, RepositoryMemberRole.WRITE);
        _repoMember2 =  OverrideId(_repoMember2, new Guid("8e9b4444-ffaa-4bf2-9f2c-5e00a21d92a9"));
        OverrideMemberList(_repository1, new List<RepositoryMember>{_repoMember1,_repoMember2});
        _branch1 = Branch.Create("branch1", _repository1.Id, false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
        _branch2 = Branch.Create("branch2", _repository1.Id, false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
        _branch3 = Branch.Create("branch3", _repository1.Id, false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));

        _branch1 = OverrideId(_branch1, new Guid("8e9b1111-ffaa-4111-9f2c-5e00a21d92a9"));
        _branch2 = OverrideId(_branch2, new Guid("8e9b1111-ffaa-4222-9f2c-5e00a21d92a9"));
        _branch3 = OverrideId(_branch3, new Guid("8e9b1111-ffaa-4333-9f2c-5e00a21d92a9"));
        _repositoryMemberRepository.Setup(x => x.FindByUserIdAndRepositoryId(_user1.Id,_repository1.Id)).ReturnsAsync(_repoMember1);
        _repositoryMemberRepository.Setup(x => x.FindByUserIdAndRepositoryId(_user2.Id,_repository1.Id)).ReturnsAsync(_repoMember2);
        
        _repositoryRepository.Setup(x => x.Find(_repository1.Id)).Returns(_repository1);

        _taskRepository.Setup(x => x.GetTaskNumber()).ReturnsAsync(1);

        _branchRepository.Setup(x=> x.FindById(_branch1.Id)).ReturnsAsync(_branch1);
        _branchRepository.Setup(x=> x.FindById(_branch2.Id)).ReturnsAsync(_branch2);
        _branchRepository.Setup(x=> x.FindById(_branch3.Id)).ReturnsAsync(_branch3);

        _userRepository.Setup(x => x.FindUserById(_user1.Id)).ReturnsAsync(_user1);
        _userRepository.Setup(x => x.FindUserById(_user2.Id)).ReturnsAsync(_user2);
        _userRepository.Setup(x => x.FindUserById(_user3.Id)).ReturnsAsync(_user3);
        
        var dummyPullRequest = PullRequest.Create("", "", 0, _repository1, new Guid(),
            new List<RepositoryMember>(), new List<Label>(), new Guid(), new Guid(), new Guid(),new List<Issue>());
        _pullRequestRepository.Setup(x => x.Create(It.IsAny<PullRequest>())).ReturnsAsync(dummyPullRequest);
        
        _pullRequestRepository.Setup(x => x.FindByBranchesAndRepository(_repository1.Id, _branch1.Id, _branch3.Id))
            .ReturnsAsync(dummyPullRequest);

    }
    
    [Fact]
    public void Handle_ShouldReturnSuccess_WhenBranchesCorrectAndUserRepositoryMember()
    {
        //Arrange
        var command = new CreatePullRequestCommand(_user1.Id,"super title","super description",_repository1.Id,
            new List<Guid>{_repoMember1.Id},new List<Guid>(),null, _branch1.Id, _branch2.Id,new List<Guid>());

        //Act
        var result = new CreatePullRequestCommandHandler(_pullRequestRepository.Object, _labelRepository.Object,
                _notificationService.Object, _repositoryRepository.Object,_taskRepository.Object, _repositoryMemberRepository.Object,
                 _branchRepository.Object,_gitService.Object, _userRepository.Object, _issueRepository.Object)
            .Handle(command,default);

        //Assert
        result.IsFaulted.ShouldBe(false);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenUserNotAMemberOfRepository()
    {
        //Arrange
        var command = new CreatePullRequestCommand(_user3.Id,"super title","super description",_repository1.Id,
            new List<Guid>{_repoMember1.Id},new List<Guid>(),null, _branch1.Id, _branch2.Id,new List<Guid>());
        var handler = new CreatePullRequestCommandHandler(_pullRequestRepository.Object, _labelRepository.Object,
            _notificationService.Object, _repositoryRepository.Object,_taskRepository.Object, _repositoryMemberRepository.Object,
            _branchRepository.Object,_gitService.Object, _userRepository.Object, _issueRepository.Object);

        //Act
        async Task<Guid> Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_henPullRequestWithSameBranchesExists()
    {
        //Arrange
        var command = new CreatePullRequestCommand(_user1.Id,"super title","super description",_repository1.Id,
            new List<Guid>{_repoMember1.Id},new List<Guid>(),null, _branch1.Id, _branch3.Id,new List<Guid>());
        var handler = new CreatePullRequestCommandHandler(_pullRequestRepository.Object, _labelRepository.Object,
            _notificationService.Object, _repositoryRepository.Object,_taskRepository.Object, _repositoryMemberRepository.Object,
            _branchRepository.Object,_gitService.Object, _userRepository.Object, _issueRepository.Object);
        //Act
        async Task<Guid> Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<PullRequestWithSameBranchesExistsException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenBranchesTheSame()
    {
        //Arrange
        var command = new CreatePullRequestCommand(_user1.Id,"super title","super description",_repository1.Id,
            new List<Guid>{_repoMember1.Id},new List<Guid>(),null, _branch1.Id, _branch1.Id,new List<Guid>());
        var handler = new CreatePullRequestCommandHandler(_pullRequestRepository.Object, _labelRepository.Object,
            _notificationService.Object, _repositoryRepository.Object,_taskRepository.Object, _repositoryMemberRepository.Object,
            _branchRepository.Object,_gitService.Object, _userRepository.Object, _issueRepository.Object);

        //Act
        async Task<Guid> Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<CantCreatePullRequestOnSameBranchException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenBranchNotFound()
    {
        //Arrange
        var command = new CreatePullRequestCommand(_user1.Id,"super title","super description",_repository1.Id,
            new List<Guid>{_repoMember1.Id},new List<Guid>(),null, new Guid(), _branch1.Id,new List<Guid>());
        var handler = new CreatePullRequestCommandHandler(_pullRequestRepository.Object, _labelRepository.Object,
            _notificationService.Object, _repositoryRepository.Object,_taskRepository.Object, _repositoryMemberRepository.Object,
            _branchRepository.Object,_gitService.Object, _userRepository.Object, _issueRepository.Object);

        //Act
        async Task<Guid> Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<BranchNotFoundException>(Handle);
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
}