using Application.Shared;
using Domain.Repositories;

namespace Application.Repositories.Queries.FindAllRepositoryMembers;

public sealed record FindAllRepositoryMembersQuery(Guid UserId, Guid RepositoryId) : IQuery<IEnumerable<RepositoryMember>>;