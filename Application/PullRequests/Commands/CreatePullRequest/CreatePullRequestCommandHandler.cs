﻿using Application.Shared;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.CreatePullRequest;

public class CreatePullRequestCommandHandler: ICommandHandler<CreatePullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly INotificationService _notificationService;
    private readonly IBranchRepository _branchRepository;
    private readonly IGitService _gitService;
    private readonly IUserRepository _userRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IMilestoneRepository _milestoneRepository;
    
    public CreatePullRequestCommandHandler (IPullRequestRepository pullRequestRepository, ILabelRepository labelRepository,
        INotificationService notificationService, IRepositoryRepository repositoryRepository, ITaskRepository taskRepository,
        IRepositoryMemberRepository repositoryMemberRepository, IBranchRepository branchRepository, IGitService gitService,
        IUserRepository userRepository, IIssueRepository issueRepository, IMilestoneRepository milestoneRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _labelRepository = labelRepository;
        _notificationService = notificationService;
        _repositoryRepository = repositoryRepository;
        _taskRepository = taskRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _branchRepository = branchRepository;
        _gitService = gitService;
        _userRepository = userRepository;
        _issueRepository = issueRepository;
        _milestoneRepository = milestoneRepository;
    }

    public async Task<Guid> Handle(CreatePullRequestCommand request, CancellationToken cancellationToken)
    {
        var member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);
        
        var repository = _repositoryRepository.Find(request.RepositoryId);
        
        var taskNumber = await _taskRepository.GetTaskNumber(request.RepositoryId) + 1;
        
        var assignees = await _repositoryMemberRepository.FindAllByIds(repository!.Id, request.AssigneesIds);
        var labels = await _labelRepository.FindAllByIds(repository.Id, request.LabelsIds);
        var issues = await _issueRepository.FindAllByIds(repository.Id, request.issueIds);

        var fromBranch = await _branchRepository.FindById(request.FromBranchId);
        Branch.ThrowIfDoesntExist(fromBranch);
        var toBranch = await _branchRepository.FindById(request.ToBranchId);
        Branch.ThrowIfDoesntExist(toBranch);
        if (fromBranch!.Id.Equals(toBranch!.Id))
            throw new CantCreatePullRequestOnSameBranchException();

        var pullRequest = await _pullRequestRepository.FindOpenByBranchesAndRepository(request.RepositoryId,fromBranch.Id,toBranch.Id);
        if (pullRequest is not null) throw new PullRequestWithSameBranchesExistsException();
        var creator = await _userRepository.FindUserById(request.UserId);

        Milestone milestone = null;
        if (request.MilestoneId != null)
            milestone = await _milestoneRepository.FindMilestone((Guid)request.MilestoneId);

        pullRequest = PullRequest.Create(request.Title, request.Description, taskNumber,
            repository, request.UserId, assignees, labels, milestone, fromBranch.Id, toBranch.Id, issues);

        var gitPullRequestId = await _gitService.CreatePullRequest(repository, fromBranch.OriginalName, toBranch.OriginalName, pullRequest);
        pullRequest.SetGitPullRequestId(gitPullRequestId);
        pullRequest = await _pullRequestRepository.Create(pullRequest);
        
        var message = $"A new Pull request has been opened in the repository {repository.Name}<br><br>" +
                      $"Title: {pullRequest.Title} <br>" +
                      $"Description: {pullRequest.Description}<br>" +
                      $"Opened by: {creator?.Username}";
        var subject = $"[Github] New Pull request opened in {repository.Name}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.PullRequests);
        
        return pullRequest.Id;
    }
}