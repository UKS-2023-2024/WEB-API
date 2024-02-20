using Application.Shared;
using Domain.Repositories.Enums;

namespace Application.Repositories.Queries.IsUserWatchingRepository;

public sealed record IsUserWatchingRepositoryQuery(Guid UserId, Guid RepositoryId) : IQuery<WatchingPreferences?>;