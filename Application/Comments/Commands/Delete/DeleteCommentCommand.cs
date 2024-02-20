using Application.Shared;

namespace Application.Comments.Commands.Delete;

public record DeleteCommentCommand(Guid CommentId): ICommand<Guid>;