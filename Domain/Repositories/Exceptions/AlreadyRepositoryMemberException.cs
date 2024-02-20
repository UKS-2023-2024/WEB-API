using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class AlreadyRepositoryMemberException: BaseException
{
    public AlreadyRepositoryMemberException() : base("User already repository member!")
    {
    }
}