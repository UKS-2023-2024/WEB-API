using Application.Shared;
using Domain.Auth;
using Domain.Repositories;
using FluentResults;

namespace Application.Repositories.Queries.FindAllRepositoriesUserBelongsTo;

public sealed record FindAllRepositoriesUserBelongsToQuery(Guid userId) : IQuery<IEnumerable<Repository>>;