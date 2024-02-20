using Application.Shared;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Milestones.Commands.Reopen;

public class ReopenMilestoneCommandHandler : ICommandHandler<ReopenMilestoneCommand, Milestone>
{
    
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;

    public ReopenMilestoneCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, IMilestoneRepository milestoneRepository,
        IRepositoryRepository repositoryRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _milestoneRepository = milestoneRepository;
        _repositoryRepository = repositoryRepository;
    }
    public async Task<Milestone> Handle(ReopenMilestoneCommand request, CancellationToken cancellationToken)
    {
        Milestone? milestone = _milestoneRepository.Find(request.MilestoneId);
        if (milestone is null) throw new MilestoneNotFoundException();
        Repository? foundRepository = _repositoryRepository.Find(milestone.RepositoryId);
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, foundRepository.Id);

        if (member is null) throw new RepositoryMemberNotFoundException();
        _milestoneRepository.Update(Milestone.Reopen(milestone));
        return milestone;
    }
}