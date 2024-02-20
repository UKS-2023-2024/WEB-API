using Application.Shared;
using Domain.Auth;

namespace Application.Auth.Queries.FindAllByStarredRepository;

public sealed record FindAllByStarredRepositoryQuery(Guid RepositoryId) : IQuery<IEnumerable<User>>;