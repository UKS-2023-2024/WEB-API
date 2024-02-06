using Application.Shared;
using Domain.Comments;
using Domain.Comments.Interfaces;

namespace Application.Comments.Commands.Create;

public class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand, Guid>
{
    private readonly ICommentRepository _commentRepository;

    public CreateCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        Comment comment = Comment.Create(request.CreatorId, DateTime.Now.ToUniversalTime(), request.Content,
            request.TaskId);
        var createdComment = await _commentRepository.Create(comment);
        return createdComment.Id;
    }
}