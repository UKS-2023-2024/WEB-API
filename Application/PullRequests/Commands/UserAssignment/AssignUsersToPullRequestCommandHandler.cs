using Application.Shared;
using Domain.Auth.Enums;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.UserAssignment;

public class AssignUsersToPullRequestCommandHandler : ICommandHandler<AssignUsersToPullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly INotificationService _notificationService;

    public AssignUsersToPullRequestCommandHandler(IPullRequestRepository pullRequestRepository, IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryRepository repositoryRepository, INotificationService notificationService)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
        _notificationService = notificationService;
    }

    public async Task<Guid> Handle(AssignUsersToPullRequestCommand request, CancellationToken cancellationToken)
    {

        PullRequest? pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);

        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);

        var repository = _repositoryRepository.Find(pullRequest.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        List<RepositoryMember> members = new();
        foreach (Guid userId in request.AssigneeIds)
        {
            var user = _repositoryMemberRepository.Find(userId);
            if (user == null) throw new RepositoryMemberNotFoundException();
            members.Add(user);
        }

        var addedUsers = members.Except(pullRequest.Assignees).ToList();
        var removedUsers = pullRequest.Assignees.Except(members).ToList();

        pullRequest.UpdateAssignees(members, member.Member.Id);
        _pullRequestRepository.Update(pullRequest);

        string message = "";
        string subject = "";
        if (addedUsers.Any())
        {
            subject += $"[Github] Users assigned to pull request #{pullRequest.Number} in {repository.Name}";
            message += $"The following users have been assigned to pull request #{pullRequest.Number} in the repository {repository.Name}:<br>";
            foreach (var user in addedUsers)
            {
                message += $"{user.Member.Username}<br>";
            }
        }

        if (removedUsers.Any())
        {
            subject += $"[Github] Users unassigned from pull request #{pullRequest.Number} in {repository.Name}";
            message += $"The following users have been unassigned from pull request #{pullRequest.Number} in the repository {repository.Name}:<br>";
            foreach (var user in removedUsers)
            {
                message += $"{user.Member.Username}<br>";
            }
        }
        message += $"<br>By: {member.Member.Username}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.PullRequests);

        return pullRequest.Id;
    }
}