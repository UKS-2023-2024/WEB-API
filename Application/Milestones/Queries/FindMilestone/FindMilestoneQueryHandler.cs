using Application.Shared;
using Domain.Milestones;
using Domain.Milestones.Interfaces;
using MediatR;

namespace Application.Milestones.Queries.FindMilestone;

public class FindMilestoneQueryHandler: IRequestHandler<FindMilestoneQuery, Milestone>
{
    private readonly IMilestoneRepository _milestoneRepository;

    public FindMilestoneQueryHandler(IMilestoneRepository milestoneRepository)
    {
        _milestoneRepository = milestoneRepository;
    }

    public async Task<Milestone> Handle(FindMilestoneQuery request, CancellationToken cancellationToken)
    {
        return await _milestoneRepository.FindMilestone(request.MilestoneId);
    }
}