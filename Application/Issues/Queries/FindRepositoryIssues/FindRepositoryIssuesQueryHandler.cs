using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.Issues.Queries.FindRepositoryIssues;

public class FindRepositoryIssuesQueryHandler: IQueryHandler<FindRepositoryIssuesQuery, List<Issue>>
{
    private readonly IIssueRepository _issueRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    
    public FindRepositoryIssuesQueryHandler(IIssueRepository issueRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _issueRepository = issueRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task<List<Issue>> Handle(FindRepositoryIssuesQuery request, CancellationToken cancellationToken)
    {
        var member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);
        return await _issueRepository.FindRepositoryIssues(request.RepositoryId);
    }
}