using Application.Shared;
using Domain.Comments;

namespace Application.Comments.Queries.GetTaskComments;

public record GetTaskCommentsQuery(Guid RepositoryId, Guid TaskId): IQuery<List<CommentHierarchy>>;