using Application.Shared;
using Domain.Branches;

namespace Application.PullRequests.Queries.GetCommitPreview;

public record GetCommitPreviewQuery(Guid Id) : IQuery<List<CommitContent>>;
    
