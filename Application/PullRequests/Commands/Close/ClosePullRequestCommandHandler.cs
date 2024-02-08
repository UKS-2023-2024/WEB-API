using Application.Shared;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.Close;

public class ClosePullRequestCommandHandler : ICommandHandler<ClosePullRequestCommand, PullRequest>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;

    public ClosePullRequestCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, IPullRequestRepository pullRequestRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _pullRequestRepository = pullRequestRepository;
    }


    public async Task<PullRequest> Handle(ClosePullRequestCommand request, CancellationToken cancellationToken)
    {
        PullRequest? pullRequest = _pullRequestRepository.Find(request.PullRequestId);
        if (pullRequest is null) throw new PullRequestNotFoundException();

        //Repository? foundRepository = _repositoryRepository.Find(pullRequest.RepositoryId);
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        if (member is null) throw new RepositoryMemberNotFoundException();

        pullRequest.ClosePullRequest(request.UserId);
        _pullRequestRepository.Update(pullRequest);
        return pullRequest;
    }
}