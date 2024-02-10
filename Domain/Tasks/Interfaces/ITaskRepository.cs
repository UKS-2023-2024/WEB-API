using Domain.Shared.Interfaces;

namespace Domain.Tasks.Interfaces;

public interface ITaskRepository: IBaseRepository<Task>
{
    public Task<int> GetTaskNumber(Guid repositoryId);
}