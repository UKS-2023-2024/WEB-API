using Application.Shared;
using Domain.Comments;
using Domain.Comments.Interfaces;

namespace Application.Comments.Commands.Delete;

public class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand, Guid>
{
    private readonly ICommentRepository _commentRepository;
    public DeleteCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Guid> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        Comment comment = _commentRepository.Find(request.CommentId);
        _commentRepository.Delete(comment);
        return comment.Id;
    }
}