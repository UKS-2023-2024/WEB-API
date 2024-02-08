using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.Reopen;

public class ReopenPullRequestCommandHandler : ICommandHandler<ReopenPullRequestCommand, PullRequest>
{
    
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;

    public ReopenPullRequestCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, IPullRequestRepository pullRequestRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _pullRequestRepository = pullRequestRepository;
    }
    public async Task<PullRequest> Handle(ReopenPullRequestCommand request, CancellationToken cancellationToken)
    {
        PullRequest? pr = _pullRequestRepository.Find(request.PullRequestId);
        if (pr is null)
            throw new PullRequestNotFoundException();
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pr.RepositoryId);
        if (member is null) throw new RepositoryMemberNotFoundException();

        pr.ReopenPullRequest(request.UserId);
        _pullRequestRepository.Update(pr);
        return pr;
    }
}