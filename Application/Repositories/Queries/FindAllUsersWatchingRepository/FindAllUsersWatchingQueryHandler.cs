using Domain.Auth;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllUsersWatchingRepository;

public class FindAllUsersWatchingQueryHandler: IRequestHandler<FindAllUsersWatchingQuery, IEnumerable<User>>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public FindAllUsersWatchingQueryHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public Task<IEnumerable<User>> Handle(FindAllUsersWatchingQuery request, CancellationToken cancellationToken)
    {
        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        repository!.ThrowIfUserCantAccessRepositoryData(request.UserId);
        return Task.FromResult<IEnumerable<User>>(repository.WatchedBy);
    }
}