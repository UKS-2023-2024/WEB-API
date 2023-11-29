using Domain.Auth;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllUsersThatStarredRepository;

public class FindAllUsersThatStarredQueryHandler: IRequestHandler<FindAllUsersThatStarredQuery, IEnumerable<User>>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public FindAllUsersThatStarredQueryHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public Task<IEnumerable<User>> Handle(FindAllUsersThatStarredQuery request, CancellationToken cancellationToken)
    {
        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        repository!.ThrowIfUserCantAccessRepositoryData(request.UserId);
        return Task.FromResult<IEnumerable<User>>(repository.StarredBy);
    }
}