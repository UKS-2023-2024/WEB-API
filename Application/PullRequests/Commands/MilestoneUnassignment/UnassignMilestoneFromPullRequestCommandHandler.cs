using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.MilestoneUnassignment;

public class UnassignMilestoneFromPullRequestCommandHandler : ICommandHandler<UnassignMilestoneFromPullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public UnassignMilestoneFromPullRequestCommandHandler(IPullRequestRepository pullRequestRepository, IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryRepository repositoryRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
    }

    public async Task<Guid> Handle(UnassignMilestoneFromPullRequestCommand request, CancellationToken cancellationToken)
    {

        PullRequest? pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);

        var member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);

        var repository = _repositoryRepository.Find(pullRequest.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);

        pullRequest.UnassignMilestone(member.Member.Id);
        
        _pullRequestRepository.Update(pullRequest);

        return pullRequest.Id;
    }
}