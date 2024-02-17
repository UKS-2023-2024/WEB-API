using Domain.Shared.Interfaces;

namespace Domain.Comments.Interfaces;

public interface ICommentRepository: IBaseRepository<Comment>
{
    Task<List<Comment>> FindAllComments();
}