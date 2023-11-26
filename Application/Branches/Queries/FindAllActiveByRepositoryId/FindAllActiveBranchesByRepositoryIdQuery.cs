using Application.Shared;
using Domain.Branches;
using Domain.Shared;

namespace Application.Repositories.Queries.FindAllActiveByRepositoryId;

public sealed record FindAllActiveBranchesByRepositoryIdQuery(Guid RepositoryId, int PageSize, int PageNumber) : IQuery<PagedResult<Branch>>;