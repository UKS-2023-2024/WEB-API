using Application.Shared;
using Domain.Auth.Enums;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.Issues.Commands.Close;

public class CloseIssueCommandHandler : ICommandHandler<CloseIssueCommand, Guid>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly INotificationService _notificationService;

    public CloseIssueCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, ITaskRepository taskRepository,
        IIssueRepository issueRepository, INotificationService notificationService)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _taskRepository = taskRepository;
        _issueRepository = issueRepository;
        _notificationService = notificationService;
    }

    public async Task<Guid> Handle(CloseIssueCommand request, CancellationToken cancellationToken)
    {
        Issue issue = await _issueRepository.FindById(request.IssueId);
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.CreatorId, issue.RepositoryId);
        if (member is null)
            throw new RepositoryMemberNotFoundException();
        issue.Close();
        _issueRepository.Update(issue);

        string message = $"Issue #{issue.Number} has been closed in the repository {issue.Repository?.Name}<br><br>" +
                       $"Title: {issue.Title} <br>" +
                       $"Description: {issue.Description}<br>" +
                       $"Closed by: {member.Member.Username}";
        string subject = $"[Github] Issue #{issue.Number} closed in {issue.Repository?.Name}";
        await _notificationService.SendNotification(issue.Repository, subject, message, NotificationType.Issues);

        return issue.Id;
    }
}