using Application.Shared;
using Domain.Auth.Enums;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.LabelAssignment;

public class AssignLabelsToPullRequestCommandHandler : ICommandHandler<AssignLabelsToPullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly INotificationService _notificationService;

    public AssignLabelsToPullRequestCommandHandler(IPullRequestRepository pullRequestRepository, IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryRepository repositoryRepository, ILabelRepository labelRepository, INotificationService notificationService)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
        _labelRepository = labelRepository;
        _notificationService = notificationService;
    }

    public async Task<Guid> Handle(AssignLabelsToPullRequestCommand request, CancellationToken cancellationToken)
    {
        PullRequest? pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);

        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);

        var repository = _repositoryRepository.Find(pullRequest.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);

        List<Label> labels = new();
        foreach (Guid labelId in request.LabelIds)
        {
            var label = _labelRepository.Find(labelId);
            labels.Add(label);
        }

        var addedLabels = labels.Except(pullRequest.Labels).ToList();
        var removedLabels = pullRequest.Labels.Except(labels).ToList();

        pullRequest.UpdateLabels(labels, member.Member.Id);
        _pullRequestRepository.Update(pullRequest);

        string message = "";
        string subject = "";
        if (addedLabels.Any())
        {
            subject += $"[Github] Labels assigned to pull request #{pullRequest.Number} in {repository.Name}";
            message += $"The following labels have been assigned to pull request #{pullRequest.Number} in the repository {repository.Name}:<br>";
            foreach (var label in addedLabels)
            {
                message += $"{label.Title}<br>";
            }
        }

        if (removedLabels.Any())
        {
            subject += $"[Github] Labels unassigned from pull request #{pullRequest.Number} in {repository.Name}";
            message += $"The following labels have been unassigned from pull request #{pullRequest.Number} in the repository {repository.Name}:<br>";
            foreach (var label in removedLabels)
            {
                message += $"#{label.Title}<br>";
            }
        }
        message += $"<br>By: {member.Member.Username}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.PullRequests);

        return pullRequest.Id;
    }
}