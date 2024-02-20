using System.Data.Entity.Migrations.Model;
using Domain.Auth;
using Domain.Exceptions;
using Domain.Organizations.Exceptions;

namespace Domain.Organizations;

public class OrganizationInvite
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid OrganizationId { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public Organization? Organization { get; private set; }
    public User? User { get; private set; }

    private OrganizationInvite(Guid userId, Guid organizationId, DateTime expiresAt)
    {
        UserId = userId;
        OrganizationId = organizationId;
        ExpiresAt = expiresAt;
    }

    public void ThrowIfExpired()
    {
        var expired = DateTime.Now.ToUniversalTime().CompareTo(ExpiresAt);
        if (expired > 0) throw new InvitationExpiredException();
    }

    public static OrganizationInvite Create(Guid memberId, Guid organizationId)
    {
        var expiresAt = DateTime.Now.AddDays(5).ToUniversalTime();
        return new OrganizationInvite(memberId, organizationId, expiresAt);
    }

    public static void ThrowIfDoesntExist(OrganizationInvite? invite)
    {
        if (invite is null) throw new OrganizationInviteNotFoundException();
    }
    
}