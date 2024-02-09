using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.Update;

public class AssignIssuesToPullRequestCommandHandler : ICommandHandler<AssignIssuesToPullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IIssueRepository _issueRepository;

    public AssignIssuesToPullRequestCommandHandler(IPullRequestRepository pullRequestRepository, IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryRepository repositoryRepository, IIssueRepository issueRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
        _issueRepository = issueRepository;
    }

    public async Task<Guid> Handle(AssignIssuesToPullRequestCommand request, CancellationToken cancellationToken)
    {

        PullRequest? pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);

        var member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);

        var repository = _repositoryRepository.Find(pullRequest.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        var issueGuids = request.IssuesIds.Select(Guid.Parse).ToList();
        List<Issue> issues = new();
        foreach (Guid issueId in issueGuids)
        {
            var issue = _issueRepository.Find(issueId);
            if (issue == null) throw new IssueNotFoundException();
            issues.Add(issue);
        }
        pullRequest.UpdateIssues(issues, member.Member.Id);
        
        _pullRequestRepository.Update(pullRequest);
        return pullRequest.Id;
    }
}