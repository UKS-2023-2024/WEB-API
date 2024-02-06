using Domain.Comments;
using Domain.Comments.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Comments;

public class CommentRepository: BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(MainDbContext context) : base(context)
    {
    }
}