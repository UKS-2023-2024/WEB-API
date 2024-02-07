using Domain.Reactions;
using Domain.Reactions.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Reactions;

public class ReactionRepository: BaseRepository<Reaction>, IReactionRepository
{
    public ReactionRepository(MainDbContext context) : base(context)
    {
    }
}