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

namespace Application.PullRequests.Commands.Reopen;

public class ReopenPullRequestCommandHandler : ICommandHandler<ReopenPullRequestCommand, PullRequest>
{
    
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly INotificationService _notificationService;
    private readonly IGitService _gitService;

    public ReopenPullRequestCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, IPullRequestRepository pullRequestRepository,
        IRepositoryRepository repositoryRepository, INotificationService notificationService, IGitService gitService)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _pullRequestRepository = pullRequestRepository;
        _repositoryRepository = repositoryRepository;
        _notificationService = notificationService;
        _gitService = gitService;
    }
    public async Task<PullRequest> Handle(ReopenPullRequestCommand request, CancellationToken cancellationToken)
    {
        PullRequest? pr = _pullRequestRepository.Find(request.PullRequestId);
        if (pr is null)
            throw new PullRequestNotFoundException();
        Repository repository = _repositoryRepository.Find(pr.RepositoryId);
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pr.RepositoryId);
        if (member is null) throw new RepositoryMemberNotFoundException();

        pr.ReopenPullRequest(request.UserId);
        _pullRequestRepository.Update(pr);
        
        _gitService.UpdatePullRequest(repository!, pr.GitPullRequestId ?? 0, "open");

        var message = $"Pull request #{pr.Number} has been reopened in the repository {repository.Name}<br><br>" +
                        $"Title: {pr.Title} <br>" +
                        $"Description: {pr.Description}<br>" +
                        $"Reopened by: {member.Member.Username}";
        var subject = $"[Github] Pull request #{pr.Number} reopened in {repository.Name}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.PullRequests);

        return pr;
    }
}