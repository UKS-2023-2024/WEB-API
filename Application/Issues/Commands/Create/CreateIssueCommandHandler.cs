using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;
using MediatR;

namespace Application.Issues.Commands.Create;

public class CreateIssueCommandHandler : ICommandHandler<CreateIssueCommand, Guid>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly INotificationService _notificationService;

    public CreateIssueCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, ITaskRepository taskRepository,
        IRepositoryRepository repositoryRepository, IIssueRepository issueRepository,
        IUserRepository userRepository, ILabelRepository labelRepository, INotificationService notificationService)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _taskRepository = taskRepository;
        _repositoryRepository = repositoryRepository;
        _issueRepository = issueRepository;
        _userRepository = userRepository;
        _labelRepository = labelRepository;
        _notificationService = notificationService;
    }

    public async Task<Guid> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        if (member is null)
            throw new RepositoryMemberNotFoundException();
        int taskNumber = await _taskRepository.GetTaskNumber(request.RepositoryId) + 1;
        Repository repository = _repositoryRepository.Find(request.RepositoryId);
        User creator = _userRepository.Find(request.UserId);
        var assigneeGuids = request.AssigneesIds.Select(s => Guid.Parse(s));
        var assignees = await _repositoryMemberRepository.FindAllByIds(repository.Id, assigneeGuids.ToList());
        var labelGuids = request.LabelsIds.Select(l => Guid.Parse(l));
        var labels = await _labelRepository.FindAllByIds(repository.Id, labelGuids.ToList());
        Issue issue = Issue.Create(request.Title, request.Description, TaskState.OPEN,
            taskNumber,
            repository, creator, assignees, labels, request.MilestoneId);
        Issue createdIssue = await _issueRepository.Create(issue);

        string message = $"A new issue has been opened in the repository {repository.Name}<br><br>" +
                         $"Title: {issue.Title} <br>" +
                         $"Description: {issue.Description}<br>" +
                         $"Opened by: {issue.Creator?.Username}";
        string subject = $"[Github] New Issue opened in {repository.Name}";
        await _notificationService.SendNotification(repository, subject, message, NotificationType.Issues);

        return createdIssue.Id;
    }
}