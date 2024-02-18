using Application.Shared;
using Domain.Comments;
using Domain.Comments.Interfaces;

namespace Application.Comments.Queries.GetTaskComments;

public class CommentHierarchy
{
    public Comment Comment { get; set; }
    public CommentHierarchy Parent { get; set; }
}

public class GetTaskCommentsQueryHandler: IQueryHandler<GetTaskCommentsQuery, List<CommentHierarchy>>
{
    private readonly ICommentRepository _commentRepository;

    public GetTaskCommentsQueryHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<List<CommentHierarchy>> Handle(GetTaskCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentRepository.FindAllByAndTaskId(request.TaskId);
        var commentHierarchy = new List<CommentHierarchy>();

        foreach (var comment in comments)
        {
            // Skip comments that are replies
            if (comment.ParentId == null)
            {
                commentHierarchy.Add(new CommentHierarchy
                {
                    Comment = comment
                });
                continue;
            }

            var hierarchy = new CommentHierarchy
            {
                Comment = comment
            };
            BuildCommentTree(comment, hierarchy);
            commentHierarchy.Add(hierarchy);
        }

        return commentHierarchy;
    }
    
    private void BuildCommentTree(Comment comment, CommentHierarchy childHierarchy)
    {
        var parentComment = _commentRepository.Find(comment.ParentId ?? new Guid());
        var hierarchy = new CommentHierarchy { Comment = parentComment!, Parent = null };
        childHierarchy.Parent = hierarchy;
        if (parentComment.ParentId is null)
            return;
        BuildCommentTree(parentComment, hierarchy);
    }

   
}