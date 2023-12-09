using Domain.Tasks.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tasks;

public class TaskRepository: BaseRepository<Domain.Tasks.Task>, ITaskRepository
{
    private readonly MainDbContext _context;
    public TaskRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> GetTaskNumber()
    {
        return await _context.Tasks.CountAsync();
    }
}