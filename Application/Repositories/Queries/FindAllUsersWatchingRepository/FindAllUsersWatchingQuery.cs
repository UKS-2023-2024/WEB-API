using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Queries.FindAllUsersWatchingRepository;

public sealed record FindAllUsersWatchingQuery(Guid UserId, Guid RepositoryId) : IQuery<IEnumerable<User>>;