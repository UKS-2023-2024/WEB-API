using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;

namespace Application.Issues.Queries.FindIssueQuery;

public class FindIssueQueryHandler: IQueryHandler<FindIssueQuery, Issue>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;

    public FindIssueQueryHandler(IIssueRepository issueRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _issueRepository = issueRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task<Issue> Handle(FindIssueQuery request, CancellationToken cancellationToken)
    {
        Issue? foundIssue = await _issueRepository.FindById(request.Id);
        if (foundIssue is null)
            throw new IssueNotFoundException();
        var member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, foundIssue.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);
        return foundIssue;
    }
}