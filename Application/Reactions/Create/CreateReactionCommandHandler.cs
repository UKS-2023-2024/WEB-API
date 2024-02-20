using Application.Shared;
using Domain.Reactions;
using Domain.Reactions.Interfaces;

namespace Application.Reactions.Create;

public class CreateReactionCommandHandler: ICommandHandler<CreateReactionCommand, Guid>
{
    public readonly IReactionRepository _reactionRepository;

    public CreateReactionCommandHandler(IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository;
    }

    public async Task<Guid> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
    {
        Reaction reaction = Reaction.Create(request.CommentId, request.CreatorId, request.EmojiCode);
        var createdReaction = await _reactionRepository.Create(reaction);
        return createdReaction.Id;
    }
}