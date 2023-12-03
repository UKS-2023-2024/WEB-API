﻿using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Milestones.Queries.FindRepositoryClosedMilestones;

public class FindRepositoryClosedMilestonesQueryHandler: IRequestHandler<FindRepositoryClosedMilestonesQuery, List<Milestone>>
{
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;

    public FindRepositoryClosedMilestonesQueryHandler(IMilestoneRepository milestoneRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _milestoneRepository = milestoneRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }
    public async Task<List<Milestone>> Handle(FindRepositoryClosedMilestonesQuery request, CancellationToken cancellationToken)
    {
        RepositoryMember? member =
            await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        if (member is null)
            throw new RepositoryMemberNotFoundException();
        List<Milestone> milestones = await _milestoneRepository.FindClosedRepositoryMilestones(request.RepositoryId);
        return milestones;
    }
}