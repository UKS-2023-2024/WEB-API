using Application.Shared;
using Domain.Branches;

namespace Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryId;

public sealed record FindAllBranchesWithoutDefaultByRepositoryIdQuery(Guid repositoryId) : IQuery<IEnumerable<Branch>>;