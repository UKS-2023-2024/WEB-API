using Application.Shared;

namespace Application.Repositories.Queries.DidUserStarRepository;

public sealed record DidUserStarRepositoryQuery(Guid UserId, Guid RepositoryId) : IQuery<bool>;