using Application.Milestones.Commands.Create;
using Application.Shared;
using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Milestones.Commands.Update;

public class UpdateMilestoneCommandHandler : ICommandHandler<UpdateMilestoneCommand, Guid>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IMilestoneRepository _milestoneRepository;

    public UpdateMilestoneCommandHandler(IRepositoryMemberRepository repositoryMemberRepository,
        IMilestoneRepository milestoneRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _milestoneRepository = milestoneRepository;
    }

    public async Task<Guid> Handle(UpdateMilestoneCommand request, CancellationToken cancellationToken)
    {
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.CreatorId, Guid.Parse(request.RepositoryId));
        if (member is null)
            throw new RepositoryMemberNotFoundException();

        Milestone? foundMilestone = _milestoneRepository.Find(request.MilestoneId);
    }
}