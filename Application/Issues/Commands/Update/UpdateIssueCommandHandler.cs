using Application.Issues.Commands.Enums;
using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Milestones;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.Issues.Commands.Update;

public class UpdateIssueCommandHandler: ICommandHandler<UpdateIssueCommand, Guid>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly INotificationService _notificationService;

    public UpdateIssueCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, ITaskRepository taskRepository,
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

    public async Task<Guid> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
    {
        var member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);
        
        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        var user = _userRepository.Find(request.UserId);
        User.ThrowIfDoesntExist(user);
        
        var issue = await _issueRepository.FindById(request.Id);
        
        if (request.Flag == UpdateIssueFlag.ASSIGNEES)
        {
            var assigneeGuids = request.AssigneesIds.Select(Guid.Parse).ToList();
            var assignees = await _repositoryMemberRepository.FindAllByIdsAndRepositoryId(repository.Id, assigneeGuids);

            var addedUsers = assignees.Except(issue.Assignees).ToList();
            var removedUsers = issue.Assignees.Except(assignees).ToList();

            issue.UpdateAssignees(assignees, user.Id);

            string message = "";
            string subject = "";
            if (addedUsers.Any())
            {
                subject += $"[Github] Users assigned to issue #{issue.Number} in {repository.Name}";
                message += $"The following users have been assigned to issue #{issue.Number} in the repository {repository.Name}:<br>";
                foreach (var assignee in addedUsers)
                {
                    message += $"{assignee.Member.Username}<br>";
                }
            }

            if (removedUsers.Any())
            {
                subject += $"[Github] Users unassigned from issue #{issue.Number} in {repository.Name}";
                message += $"The following users have been unassigned from pull request #{issue.Number} in the repository {repository.Name}:<br>";
                foreach (var assignee in removedUsers)
                {
                    message += $"{assignee.Member.Username}<br>";
                }
            }
            message += $"<br>By: {member.Member.Username}";
            await _notificationService.SendNotification(repository, subject, message, NotificationType.Issues);
        }

        if (request.Flag == UpdateIssueFlag.MILESTONE_ASSIGNED)
        {
            issue.UpdateMilestone(request.MilestoneId.GetValueOrDefault(), user.Id);

            var message = $"Milestone has been assigned to issue #{issue.Number} in the repository {repository.Name}<br>" +
                     $"Assigned by: {member.Member.Username}";
            var subject = $"[Github] Milestone assigned to issue #{issue.Number} in {repository.Name}";
            await _notificationService.SendNotification(repository, subject, message, NotificationType.Issues);

        }

        if (request.Flag == UpdateIssueFlag.MILESTONE_UNASSIGNED)
        {
            issue.UnassignMilestone(request.MilestoneId.GetValueOrDefault(), user.Id, issue.MilestoneId.GetValueOrDefault());

            var message = $"Milestone has been unassigned from issue #{issue.Number} in the repository {repository.Name}<br>" +
                      $"Unassigned by: {member.Member.Username}";
            var subject = $"[Github] Milestone unassigned from issue #{issue.Number} in {repository.Name}";
            await _notificationService.SendNotification(repository, subject, message, NotificationType.Issues);
        }

        if (request.Flag == UpdateIssueFlag.LABEL_ASSIGNED)
        {
            Label newLabel = _labelRepository.Find(Guid.Parse(request.LabelsIds[request.LabelsIds.Count-1]));
            // Label createdLabel = Label.Create(newLabel.Title, newLabel.Description, newLabel.Color,
            //     newLabel.RepositoryId, false);
            // await _labelRepository.Create(createdLabel);
            issue.AssignLabel(newLabel, user.Id);
            _issueRepository.Update(issue);

            string subject = $"[Github] Label assigned from issue #{issue.Number} in {repository.Name}";
            string message = $"Label {newLabel.Title} has been assigned from pull request #{issue.Number} in the repository {repository.Name}:<br>";
            await _notificationService.SendNotification(repository, subject, message, NotificationType.Issues);
        }
        
        if (request.Flag == UpdateIssueFlag.LABEL_UNASSIGNED)
        {
            Label foundLabel = _labelRepository.Find(Guid.Parse(request.LabelsIds[0]));
            issue.UnassignLabel(foundLabel, user.Id);
            _issueRepository.Update(issue);

            string subject = $"[Github] Label unassigned from issue #{issue.Number} in {repository.Name}";
            string message = $"Label {foundLabel.Title} has been unassigned from pull request #{issue.Number} in the repository {repository.Name}:<br>";
            await _notificationService.SendNotification(repository, subject, message, NotificationType.Issues);
        }
     

        //var labelGuids = request.LabelsIds.Select(l => Guid.Parse(l));
        //var labels = await _labelRepository.FindAllByIds(repository.Id, labelGuids.ToList());
        //Issue updatedIssue = Issue.Update(foundIssue, request.Title, request.Description, request.State,assignees , request.MilestoneId, labels);
        _issueRepository.Update(issue);
        return issue.Id;
    }
}