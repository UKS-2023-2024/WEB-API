using Application.Issues.Commands.Enums;
using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
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

    public UpdateIssueCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, ITaskRepository taskRepository,
        IRepositoryRepository repositoryRepository, IIssueRepository issueRepository,
        IUserRepository userRepository, ILabelRepository labelRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _taskRepository = taskRepository;
        _repositoryRepository = repositoryRepository;
        _issueRepository = issueRepository;
        _userRepository = userRepository;
        _labelRepository = labelRepository;
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
            issue.UpdateAssignees(assignees, user.Id);
        }

        if (request.Flag == UpdateIssueFlag.MILESTONE_ASSIGNED)
        {
            issue.UpdateMilestone(request.MilestoneId.GetValueOrDefault(), user.Id);
        }

        if (request.Flag == UpdateIssueFlag.MILESTONE_UNASSIGNED)
        {
            issue.UnassignMilestone(request.MilestoneId.GetValueOrDefault(), user.Id, issue.MilestoneId.GetValueOrDefault());
        }

        //var labelGuids = request.LabelsIds.Select(l => Guid.Parse(l));
        //var labels = await _labelRepository.FindAllByIds(repository.Id, labelGuids.ToList());
        //Issue updatedIssue = Issue.Update(foundIssue, request.Title, request.Description, request.State,assignees , request.MilestoneId, labels);
        _issueRepository.Update(issue);
        return issue.Id;
    }
}