using Application.Shared;
using Domain.Branches;

namespace Application.Repositories.Queries.FindAllNotDeletedByRepositoryId;

public sealed record FindDefaultBranchByRepositoryIdQuery(Guid repositoryId) : IQuery<Branch>;