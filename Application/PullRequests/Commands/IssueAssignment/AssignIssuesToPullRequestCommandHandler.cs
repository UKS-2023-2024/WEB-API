using Application.Shared;
using Domain.Auth.Enums;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.IssueAssignment;

public class AssignIssuesToPullRequestCommandHandler : ICommandHandler<AssignIssuesToPullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly INotificationService _notificationService;

    public AssignIssuesToPullRequestCommandHandler(IPullRequestRepository pullRequestRepository, IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryRepository repositoryRepository, IIssueRepository issueRepository, INotificationService notificationService)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
        _issueRepository = issueRepository;
        _notificationService = notificationService;
    }

    public async Task<Guid> Handle(AssignIssuesToPullRequestCommand request, CancellationToken cancellationToken)
    {
        PullRequest? pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);

        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);

        var repository = _repositoryRepository.Find(pullRequest.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);

        List<Issue> issues = new();
        foreach (Guid issueId in request.IssuesIds)
        {
            var issue = _issueRepository.Find(issueId);
            if (issue == null) throw new IssueNotFoundException();
            issues.Add(issue);
        }

        var addedIssues = issues.Except(pullRequest.Issues).ToList();
        var removedIssues = pullRequest.Issues.Except(issues).ToList();

        pullRequest.UpdateIssues(issues, member.Member.Id);
        _pullRequestRepository.Update(pullRequest);

        string message = "";
        string subject = "";
        if (addedIssues.Any())
        {
            subject += $"[Github] Issues assigned to pull request #{pullRequest.Number} in {repository.Name}";
            message += $"The following issues have been assigned to pull request #{pullRequest.Number} in the repository {repository.Name}:<br>";
            foreach (var issue in addedIssues)
            {
                message += $"#{issue.Number} {issue.Title}<br>";
            }
        }

        if (removedIssues.Any())
        {
            subject += $"[Github] Issues unassigned from pull request #{pullRequest.Number} in {repository.Name}";
            message += $"The following issues have been unassigned from pull request #{pullRequest.Number} in the repository {repository.Name}:<br>";
            foreach (var issue in removedIssues)
            {
                message += $"#{issue.Number} {issue.Title}<br>";
            }
        }
        message += $"<br>By: {member.Member.Username}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.PullRequests);

        return pullRequest.Id;
    }
}