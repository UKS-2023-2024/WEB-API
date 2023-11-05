using Application.Shared;
using Domain.Auth;
using Domain.Repositories;
using FluentResults;

namespace Application.Repositories.Queries.FindAllByOrganizationId;

public sealed record FindAllRepositoriesByOrganizationIdQuery(Guid organizationId) : IQuery<IEnumerable<Repository>>;