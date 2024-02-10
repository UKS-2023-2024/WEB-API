using Application.Shared;
using Domain.Auth.Enums;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.MilestoneUnassignment;

public class UnassignMilestoneFromPullRequestCommandHandler : ICommandHandler<UnassignMilestoneFromPullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly INotificationService _notificationService;

    public UnassignMilestoneFromPullRequestCommandHandler(IPullRequestRepository pullRequestRepository, IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryRepository repositoryRepository, INotificationService notificationService)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
        _notificationService = notificationService;
    }

    public async Task<Guid> Handle(UnassignMilestoneFromPullRequestCommand request, CancellationToken cancellationToken)
    {
        PullRequest? pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);

        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);

        var repository = _repositoryRepository.Find(pullRequest.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);

        pullRequest.UnassignMilestone(member.Member.Id);

        _pullRequestRepository.Update(pullRequest);

        var message = $"Milestone has been unassigned from pull request #{pullRequest.Number} in the repository {repository.Name}<br>" +
                      $"Unassigned by: {member.Member.Username}";
        var subject = $"[Github] Milestone unassigned from pull request #{pullRequest.Number} in {repository.Name}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.PullRequests);

        return pullRequest.Id;
    }
}