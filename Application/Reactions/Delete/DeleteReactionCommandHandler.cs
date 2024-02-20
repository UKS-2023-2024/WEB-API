using Application.Shared;
using Domain.Reactions;
using Domain.Reactions.Interfaces;

namespace Application.Reactions.Delete;

public class DeleteReactionCommandHandler: ICommandHandler<DeleteReactionCommand, Guid>
{
    public readonly IReactionRepository _reactionRepository;

    public DeleteReactionCommandHandler(IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository;
    }
    
    public async Task<Guid> Handle(DeleteReactionCommand request, CancellationToken cancellationToken)
    {
        Reaction reaction = _reactionRepository.Find(request.ReactionId);
        _reactionRepository.Delete(reaction);
        return reaction.Id;
    }
}