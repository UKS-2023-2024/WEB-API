using Application.Shared;
using Domain.Auth;
using Domain.Repositories;

namespace Application.Repositories.Queries.FindAllUsersWatchingRepository;

public sealed record FindAllUsersWatchingQuery(Guid UserId, Guid RepositoryId) : IQuery<IEnumerable<RepositoryWatcher>>;