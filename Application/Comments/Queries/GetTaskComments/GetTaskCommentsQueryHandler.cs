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
        var comments = await _commentRepository.FindAllComments();
        var commentHierarchy = new List<CommentHierarchy>();

        foreach (var comment in comments)
        {
            // Skip comments that are replies
            if (comment.ParentId == null)
                continue;

            var hierarchy = new CommentHierarchy
            {
                Comment = comment
            };
            BuildCommentTree(comment, hierarchy);
            commentHierarchy.Add(hierarchy);
        }

        return commentHierarchy;
    }
    
    private async void BuildCommentTree(Comment comment, CommentHierarchy childHierarchy)
    {
        List<Comment> comments = await _commentRepository.FindAllComments();
        foreach (var parentComment in comments.Where(c => c.Id == comment.ParentId))
        {
            var hierarchy = new CommentHierarchy { Comment = parentComment, Parent = null };
            childHierarchy.Parent = hierarchy;
            BuildCommentTree(parentComment, hierarchy);
        }
    }

   
}