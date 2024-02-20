using Domain.Shared.Interfaces;

namespace Domain.Comments.Interfaces;

public interface ICommentRepository: IBaseRepository<Comment>
{
    Task<Comment> FindByParentId(Guid parentId);
    Task<List<Comment>> FindAllByAndTaskId(Guid taskId);
}