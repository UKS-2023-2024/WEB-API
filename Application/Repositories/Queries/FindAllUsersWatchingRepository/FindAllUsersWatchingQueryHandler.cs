using Domain.Auth;
using Domain.Repositories;
using Domain.Repositories.Enums;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllUsersWatchingRepository;

public class FindAllUsersWatchingQueryHandler: IRequestHandler<FindAllUsersWatchingQuery, IEnumerable<RepositoryWatcher>>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public FindAllUsersWatchingQueryHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public Task<IEnumerable<RepositoryWatcher>> Handle(FindAllUsersWatchingQuery request, CancellationToken cancellationToken)
    {
        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        repository!.ThrowIfUserCantAccessRepositoryData(request.UserId);
        var watchers = repository.WatchedBy.Where(w => w.WatchingPreferences == WatchingPreferences.AllActivity).ToList();
        return Task.FromResult<IEnumerable<RepositoryWatcher>>(watchers);
    }
}