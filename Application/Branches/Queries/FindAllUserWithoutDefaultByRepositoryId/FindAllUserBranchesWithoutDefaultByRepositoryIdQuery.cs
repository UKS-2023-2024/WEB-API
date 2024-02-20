using Application.Shared;
using Domain.Branches;
using Domain.Shared;

namespace Application.Repositories.Queries.FindAllUserWithoutDefaultByRepositoryId;

public sealed record FindAllUserBranchesWithoutDefaultByRepositoryIdQuery(Guid RepositoryId, Guid OwnerId, int PageSize, int PageNumber) : IQuery<PagedResult<Branch>>;