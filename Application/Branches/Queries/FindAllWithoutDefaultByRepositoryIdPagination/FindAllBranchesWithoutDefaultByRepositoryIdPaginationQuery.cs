using Application.Shared;
using Domain.Branches;
using Domain.Shared;

namespace Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryIdPagination;

public sealed record FindAllBranchesWithoutDefaultByRepositoryIdPaginationQuery(Guid RepositoryId, int PageSize, int PageNumber) : IQuery<PagedResult<Branch>>;