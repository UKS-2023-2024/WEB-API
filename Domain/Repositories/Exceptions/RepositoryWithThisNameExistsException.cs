using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;


public class RepositoryWithThisNameExistsException : BaseException
{
    public RepositoryWithThisNameExistsException() : base("Repository with this name already exists!")
    {
    }
}