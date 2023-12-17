using Application.Shared;
using Domain.Tasks;

namespace Application.Issues.Queries.FindIssueQuery;

public record FindIssueQuery(Guid UserId, Guid Id): IQuery<Issue>;