using Application.Shared;
using Domain.Tasks;

namespace Application.Issues.Queries.FindRepositoryIssues;

public record FindRepositoryIssuesQuery(Guid UserId, Guid RepositoryId): IQuery<List<Issue>>;