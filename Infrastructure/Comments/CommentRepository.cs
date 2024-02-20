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

    public Task<Comment> FindByParentId(Guid parentId)
    {
        return _context.Comments.Where(x => x.Id == parentId)
            .Include(c => c.Creator)
            .Include(c => c.Reactions)
            .FirstAsync();
    }

    public async Task<List<Comment>> FindAllByAndTaskId(Guid taskId)
    {
        return _context.Comments.Where(x => x.TaskId == taskId)
            .Include(c => c.Creator)
            .Include(c => c.Reactions)
            .ToList();
    }
}