using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryInviteNotFound: BaseException
{
    public RepositoryInviteNotFound() : base("Invite not found!")
    {
    }
}