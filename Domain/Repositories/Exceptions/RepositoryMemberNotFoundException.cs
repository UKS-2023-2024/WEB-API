using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryMemberNotFoundException : BaseException
{
    public RepositoryMemberNotFoundException() : base("Repository member not found!")
    {
    }
}