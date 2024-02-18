using Application.Shared;
using Domain.Auth.Enums;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.Close;

public class ClosePullRequestCommandHandler : ICommandHandler<ClosePullRequestCommand, PullRequest>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly INotificationService _notificationService;
    private readonly IGitService _gitService;

    public ClosePullRequestCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, IPullRequestRepository pullRequestRepository
    , IRepositoryRepository repositoryRepository, INotificationService notificationService, IGitService gitService)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _pullRequestRepository = pullRequestRepository;
        _repositoryRepository = repositoryRepository;
        _notificationService = notificationService;
        _gitService = gitService;
    }


    public async Task<PullRequest> Handle(ClosePullRequestCommand request, CancellationToken cancellationToken)
    {
        PullRequest? pullRequest = _pullRequestRepository.Find(request.PullRequestId);
        if (pullRequest is null) throw new PullRequestNotFoundException();

        Repository? repository = _repositoryRepository.Find(pullRequest.RepositoryId);
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        if (member is null) throw new RepositoryMemberNotFoundException();

        pullRequest.ClosePullRequest(request.UserId);
        _pullRequestRepository.Update(pullRequest);

        await _gitService.UpdatePullRequest(repository!, pullRequest.GitPullRequestId ?? 0, "closed");

        var message = $"Pull request #{pullRequest.Number} has been closed in the repository {repository.Name}<br><br>" +
                      $"Title: {pullRequest.Title} <br>" +
                      $"Description: {pullRequest.Description}<br>" +
                      $"Closed by: {member.Member.Username}";
        var subject = $"[Github] Pull request #{pullRequest.Number} closed in {repository.Name}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.PullRequests);

        return pullRequest;
    }
}