using Application.Shared;
using Domain.Repositories;

namespace Application.Repositories.Queries.FindRepositoryMemberRole;

public sealed record FindRepositoryMemberRoleQuery(Guid UserId, Guid RepositoryId) : IQuery<RepositoryMemberRole?>;