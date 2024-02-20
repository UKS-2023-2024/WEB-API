using Application.Shared;

namespace Application.Reactions.Delete;

public record DeleteReactionCommand(Guid ReactionId): ICommand<Guid>;