using Application.Shared;

namespace Application.Reactions.Create;

public record CreateReactionCommand(Guid CreatorId, Guid CommentId, string EmojiCode): ICommand<Guid>;