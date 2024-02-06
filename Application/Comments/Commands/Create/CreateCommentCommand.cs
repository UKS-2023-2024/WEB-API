using Application.Shared;

namespace Application.Comments.Commands.Create;

public record CreateCommentCommand(Guid CreatorId, Guid TaskId, string Content): ICommand<Guid>;