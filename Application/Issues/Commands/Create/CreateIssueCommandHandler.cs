using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;

namespace Application.Issues.Commands.Create;

public class CreateIssueCommandHandler : ICommandHandler<CreateIssueCommand, Guid>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILabelRepository _labelRepository;
    private readonly IEmailService _emailService;

    public CreateIssueCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, ITaskRepository taskRepository,
        IRepositoryRepository repositoryRepository, IIssueRepository issueRepository,
        IUserRepository userRepository, ILabelRepository labelRepository, IEmailService emailService)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _taskRepository = taskRepository;
        _repositoryRepository = repositoryRepository;
        _issueRepository = issueRepository;
        _userRepository = userRepository;
        _labelRepository = labelRepository;
        _emailService = emailService;
    }

    public async Task<Guid> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        if (member is null)
            throw new RepositoryMemberNotFoundException();
        int taskNumber = await _taskRepository.GetTaskNumber() + 1;
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
        

        foreach(User watcher in repository.WatchedBy)
        {
            await _emailService.SendNotificationIssueIsOpen(watcher, issue, repository.Name);
        }

        return createdIssue.Id;
    }
}