using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class YouAlreadyHaveRepositoryWithThisNameException:BaseException
{
    public YouAlreadyHaveRepositoryWithThisNameException() : base("You already have repository with this name!")
    {
    }
}