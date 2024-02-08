using Application.Shared;
using Domain.Branches;

namespace Application.Branches.Queries.FindAllRepositoryBranches;

public record FindAllRepositoryBranchesQuery(Guid UserId, Guid RepositoryId): IQuery<IEnumerable<Branch>>;