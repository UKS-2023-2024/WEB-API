using Application.Shared;
using Domain.Tasks;

namespace Application.Issues.Queries.FindIssueEventsQuery;

public record FindIssueEventsQuery(Guid UserId, Guid Id): IQuery<List<Event>>;