using Application.Shared;
using Domain.Branches;

namespace Application.Branches.Queries.ListCommits;

public record ListCommitsQuery(Guid Branch) : IQuery<List<CommitContent>>;
