using Application.Shared;
using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Milestones.Commands.Create;

public class CreateMilestoneCommandHandler : ICommandHandler<CreateMilestoneCommand, Guid>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IMilestoneRepository _milestoneRepository;

    public CreateMilestoneCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, IMilestoneRepository milestoneRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _milestoneRepository = milestoneRepository;
    }

    public async Task<Guid> Handle(CreateMilestoneCommand request, CancellationToken cancellationToken)
    {
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.CreatorId, Guid.Parse(request.RepositoryId));
        if (member is null)
            throw new RepositoryMemberNotFoundException();

        Milestone milestone =
            Milestone.Create(request.Title, request.Description, request.DueDate, Guid.Parse(request.RepositoryId));

        Milestone createdMilestone = await _milestoneRepository.Create(milestone);
        return createdMilestone.Id;
    }
}