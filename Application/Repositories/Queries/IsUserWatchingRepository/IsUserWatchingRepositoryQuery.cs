using Application.Shared;

namespace Application.Repositories.Queries.IsUserWatchingRepository;

public sealed record IsUserWatchingRepositoryQuery(Guid UserId, Guid RepositoryId) : IQuery<bool>;