using Application.Shared;
using Domain.Branches;

namespace Application.Repositories.Queries.FindAllNotDeletedByRepositoryId;

public sealed record FindAllNotDeletedBranchesByRepositoryIdQuery(Guid repositoryId) : IQuery<IEnumerable<Branch>>;