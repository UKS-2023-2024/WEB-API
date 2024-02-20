using Application.Shared;
using Domain.Branches;

namespace Application.Repositories.Queries.FindDefaultBranchByRepositoryId;

public sealed record FindDefaultBranchByRepositoryIdQuery(Guid repositoryId) : IQuery<Branch>;