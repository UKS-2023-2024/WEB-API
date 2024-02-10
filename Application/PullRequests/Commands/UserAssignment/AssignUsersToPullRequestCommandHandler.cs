using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Tasks;
using Domain.Tasks.Interfaces;

namespace Application.PullRequests.Commands.UserAssignment;

public class AssignUsersToPullRequestCommandHandler : ICommandHandler<AssignUsersToPullRequestCommand, Guid>
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public AssignUsersToPullRequestCommandHandler(IPullRequestRepository pullRequestRepository, IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryRepository repositoryRepository)
    {
        _pullRequestRepository = pullRequestRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
    }

    public async Task<Guid> Handle(AssignUsersToPullRequestCommand request, CancellationToken cancellationToken)
    {

        PullRequest? pullRequest = _pullRequestRepository.Find(request.Id);
        PullRequest.ThrowIfDoesntExist(pullRequest);

        var member = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, pullRequest.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(member);

        var repository = _repositoryRepository.Find(pullRequest.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        List<RepositoryMember> members = new();
        foreach (Guid userId in request.AssigneeIds)
        {
            var user = _repositoryMemberRepository.Find(userId);
            if (user == null) throw new RepositoryMemberNotFoundException();
            members.Add(user);
        }
        pullRequest.UpdateAssignees(members, member.Member.Id);
        
        _pullRequestRepository.Update(pullRequest);
        return pullRequest.Id;
    }
}