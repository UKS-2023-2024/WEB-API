using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands;

public class CreatePullRequestCommandHandler: ICommandHandler<CreatePullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly INotificationService _notificationService;
    private readonly IBranchRepository _branchRepository;
    
    public CreatePullRequestCommandHandler (IPullRequestRepository pullRequestRepository, ILabelRepository labelRepository,
        INotificationService notificationService, IRepositoryRepository repositoryRepository, ITaskRepository taskRepository,
        IRepositoryMemberRepository repositoryMemberRepository, IBranchRepository branchRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _labelRepository = labelRepository;
        _notificationService = notificationService;
        _repositoryRepository = repositoryRepository;
        _taskRepository = taskRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _branchRepository = branchRepository;
    }

    public async Task<Guid> Handle(CreatePullRequestCommand request, CancellationToken cancellationToken)
    {
        var member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);

        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        var taskNumber = await _taskRepository.GetTaskNumber() + 1;
        
        var assignees = await _repositoryMemberRepository.FindAllByIds(repository!.Id, request.AssigneesIds);
        var labels = await _labelRepository.FindAllByIds(repository.Id, request.LabelsIds);

        var fromBranch = await _branchRepository.FindById(request.FromBranchId);
        Branch.ThrowIfDoesntExist(fromBranch);
        var toBranch = await _branchRepository.FindById(request.ToBranchId);
        Branch.ThrowIfDoesntExist(toBranch);
        if (fromBranch!.Id.Equals(toBranch!.Id))
            throw new CantCreatePullRequestOnSameBranchException();
        
        var pullRequest = PullRequest.Create(request.Title, request.Description, TaskState.OPEN, taskNumber,
            repository, request.UserId, assignees, labels, request.MilestoneId,fromBranch.Id, toBranch.Id);
        pullRequest = await _pullRequestRepository.Create(pullRequest);

        var message = $"A new Pull request has been opened in the repository {repository.Name}<br><br>" +
                      $"Title: {pullRequest.Title} <br>" +
                      $"Description: {pullRequest.Description}<br>" +
                      $"Opened by: {pullRequest.Creator?.Username}";
        var subject = $"[Github] New Pull request opened in {repository.Name}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.PullRequests);

        return pullRequest.Id;
    }
}