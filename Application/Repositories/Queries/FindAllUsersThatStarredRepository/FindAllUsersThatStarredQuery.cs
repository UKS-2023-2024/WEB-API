using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Queries.FindAllUsersThatStarredRepository;

public sealed record FindAllUsersThatStarredQuery(Guid UserId, Guid RepositoryId) : IQuery<IEnumerable<User>>;