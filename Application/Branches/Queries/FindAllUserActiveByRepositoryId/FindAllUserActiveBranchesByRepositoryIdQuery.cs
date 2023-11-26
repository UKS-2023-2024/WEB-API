using Application.Shared;
using Domain.Branches;
using Domain.Shared;

namespace Application.Repositories.Queries.FindAllUserActiveByRepositoryId;

public sealed record FindAllUserActiveBranchesByRepositoryIdQuery(Guid RepositoryId, Guid OwnerId, int PageSize, int PageNumber) : IQuery<PagedResult<Branch>>;