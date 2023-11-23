using Application.Shared;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Milestones.Commands.Delete;

public class DeleteMilestoneCommandHandler: ICommandHandler<DeleteMilestoneCommand, Guid>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public DeleteMilestoneCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, 
        IMilestoneRepository milestoneRepository, IRepositoryRepository repositoryRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _milestoneRepository = milestoneRepository;
        _repositoryRepository = repositoryRepository;
    }

    public async Task<Guid> Handle(DeleteMilestoneCommand request, CancellationToken cancellationToken)
    {
        Milestone? milestone = _milestoneRepository.Find(request.MilestoneId);
        if (milestone is null) throw new MilestoneNotFoundException();
        Repository? foundRepository = _repositoryRepository.Find(milestone.RepositoryId);
        RepositoryMember? member =
           await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, foundRepository.Id);

        if (member is null) throw new RepositoryMemberNotFoundException();
        _milestoneRepository.Delete(milestone);
        return request.MilestoneId;
    }
}