using Application.Shared;
using Domain.Auth;
using Domain.Repositories;
using FluentResults;

namespace Application.Repositories.Queries.FindAllByOwnerId;

public sealed record FindAllRepositoriesByOwnerIdQuery(Guid ownerId) : IQuery<IEnumerable<Repository>>;