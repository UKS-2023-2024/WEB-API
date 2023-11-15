using Domain.Auth;
using Domain.Organizations.Exceptions;
using Domain.Repositories.Exceptions;

namespace Domain.Repositories;

public class RepositoryInvite
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid RepositoryId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public Repository? Repository { get; private set; }
    public User? User { get; private set; }
    
    private RepositoryInvite(Guid userId, Guid repositoryId, DateTime expiresAt)
    {
        UserId = userId;
        RepositoryId = repositoryId;
        ExpiresAt = expiresAt;
    }
    
    public static RepositoryInvite Create(Guid userId, Guid repositoryId)
    {
        var expiresAt = DateTime.Now.AddDays(5).ToUniversalTime();
        return new RepositoryInvite(userId, repositoryId, expiresAt);
    }

    public void ThrowIfExpired()
    {
        var expired = DateTime.Now.ToUniversalTime().CompareTo(ExpiresAt);
        if (expired > 0) throw new InvitationExpiredException();
    }
    
    public static void ThrowIfDoesntExist(RepositoryInvite? user)
    {
        if (user is null) throw new RepositoryInviteNotFound();
    }
    
    public void ThrowIfNotAnOwner(Guid userId)
    {
        if (!userId.Equals(UserId)) throw new NotInviteOwnerException();
    }
}