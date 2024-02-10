using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.MilestoneAssignment;

public class AssignMilestoneToPullRequestCommandHandler : ICommandHandler<AssignMilestoneToPullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IMilestoneRepository _milestoneRepository;

    public AssignMilestoneToPullRequestCommandHandler(IPullRequestRepository pullRequestRepository, IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryRepository repositoryRepository, IMilestoneRepository milestoneRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
        _milestoneRepository = milestoneRepository;
    }

    public async Task<Guid> Handle(AssignMilestoneToPullRequestCommand request, CancellationToken cancellationToken)
    {

        PullRequest? pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);

        var member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);

        var repository = _repositoryRepository.Find(pullRequest.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        Milestone? milestone = _milestoneRepository.Find(request.MilestoneId ?? new Guid());
        if (milestone == null) throw new MilestoneNotFoundException();

        pullRequest.UpdateMilestone(milestone, member.Member.Id);
        
        _pullRequestRepository.Update(pullRequest);

        return pullRequest.Id;
    }
}