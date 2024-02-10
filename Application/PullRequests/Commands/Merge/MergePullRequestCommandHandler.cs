using Application.Shared;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Application.PullRequests.Commands.Merge;

public class MergePullRequestCommandHandler : ICommandHandler<MergePullRequestCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly INotificationService _notificationService;
    private readonly IGitService _gitService;

    public MergePullRequestCommandHandler(IRepositoryRepository repositoryRepository,
        IPullRequestRepository pullRequestRepository,
        IRepositoryMemberRepository repositoryMemberRepository,
        IGitService gitService, 
        INotificationService notificationService)
    {
        _repositoryRepository = repositoryRepository;
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _gitService = gitService;
        _notificationService = notificationService;
    }

    public async Task Handle(MergePullRequestCommand request, CancellationToken cancellationToken)
    {
        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        var repoMember =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(repoMember);

            var pullRequest = await _pullRequestRepository.FindByIdAndRepositoryId(request.RepositoryId,request.PullRequestId);
        PullRequest.ThrowIfDoesntExist(pullRequest);
        
        pullRequest!.MergePullRequest(request.UserId);

        await _gitService.MergePullRequest(repository, request.MergeType.ToString("G").ToLower(),pullRequest.GitPullRequestId ?? 0);
        _pullRequestRepository.Update(pullRequest);

        var message =
            $"A Pull request {pullRequest.Number} has been merged in the repository {repository.Name}<br><br>" +
            $"Title: {pullRequest.Title} <br>" +
            $"Description: {pullRequest.Description}<br>";
        var subject = $"[Github] Pull request merged in {repository.Name}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.PullRequests);
    }
}