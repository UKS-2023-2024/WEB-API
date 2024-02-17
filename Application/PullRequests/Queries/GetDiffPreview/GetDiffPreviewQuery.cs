using Application.Shared;

namespace Application.PullRequests.Queries.GetDiffPreview;

public record GetDiffPreviewQuery(Guid Id): IQuery<string>;