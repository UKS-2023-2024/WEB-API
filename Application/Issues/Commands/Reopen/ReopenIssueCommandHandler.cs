using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.Issues.Commands.Reopen;

public class ReopenIssueCommandHandler : ICommandHandler<ReopenIssueCommand, Guid>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IIssueRepository _issueRepository;

    public ReopenIssueCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, ITaskRepository taskRepository,
        IIssueRepository issueRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _taskRepository = taskRepository;
        _issueRepository = issueRepository;
    }
    
    public async Task<Guid> Handle(ReopenIssueCommand request, CancellationToken cancellationToken)
    {
        Issue issue = await _issueRepository.FindById(request.IssueId);
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.CreatorId, issue.RepositoryId);
        if (member is null)
            throw new RepositoryMemberNotFoundException();
        issue.Reopen();
        _issueRepository.Update(issue);
        return issue.Id;
    }
}