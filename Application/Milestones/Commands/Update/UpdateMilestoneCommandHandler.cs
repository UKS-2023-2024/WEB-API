using Application.Milestones.Commands.Create;
using Application.Shared;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Milestones.Commands.Update;

public class UpdateMilestoneCommandHandler : ICommandHandler<UpdateMilestoneCommand, Milestone>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IMilestoneRepository _milestoneRepository;

    public UpdateMilestoneCommandHandler(IRepositoryMemberRepository repositoryMemberRepository,
        IMilestoneRepository milestoneRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _milestoneRepository = milestoneRepository;
    }

    public async Task<Milestone> Handle(UpdateMilestoneCommand request, CancellationToken cancellationToken)
    {
        Milestone? foundMilestone = _milestoneRepository.Find(request.MilestoneId);
        if (foundMilestone is null)
            throw new MilestoneNotFoundException();
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.CreatorId,
                foundMilestone.RepositoryId);
        if (member is null)
            throw new RepositoryMemberNotFoundException();
    
        Milestone updatedMilestone =
            Milestone.Update(foundMilestone, request.Title, request.Description, request.DueDate);
        _milestoneRepository.Update(updatedMilestone);
        return updatedMilestone;
    }
    
}