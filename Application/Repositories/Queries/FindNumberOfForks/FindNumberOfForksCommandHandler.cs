using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Queries.FindNumberOfForks;

public class FindNumberOfForksCommandHandler : ICommandHandler<FindNumberOfForksCommand, int>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryForkRepository _repositoryForkRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public FindNumberOfForksCommandHandler(IRepositoryMemberRepository repositoryMemberRepository,
        IRepositoryForkRepository repositoryForkRepository,
        IRepositoryRepository repositoryRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryForkRepository = repositoryForkRepository;
        _repositoryRepository = repositoryRepository;
    }

    public async Task<int> Handle(FindNumberOfForksCommand request, CancellationToken cancellationToken)
    {
        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        if (repository!.IsPrivate)
        {
            var repositoryMember =
                await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
            RepositoryMember.ThrowIfDoesntExist(repositoryMember);
        }
        return await _repositoryForkRepository.FindNumberOfForksForRepository(request.RepositoryId);
    }
}