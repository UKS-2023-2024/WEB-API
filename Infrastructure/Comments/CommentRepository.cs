using Domain.Comments;
using Domain.Comments.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Comments;

public class CommentRepository: BaseRepository<Comment>, ICommentRepository
{
    private readonly MainDbContext _context;
    public CommentRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Comment>> FindAllComments()
    {
        return _context.Comments.Where(x => x.Id != null)
            .Include(c => c.Creator)
            .Include(c => c.Reactions)
            .ToList();
    }
}