using System.Data.Entity.Migrations.Model;
using Domain.Exceptions;
using Domain.Organizations.Exceptions;

namespace Domain.Organizations;

public class OrganizationInvite
{
    public Guid MemberId { get; private set; }
    public Guid OrganizationId { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public OrganizationMember? OrganizationMember { get; private set; }

    private OrganizationInvite(Guid memberId, Guid organizationId, string token, DateTime expiresAt)
    {
        MemberId = memberId;
        OrganizationId = organizationId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public void ThrowIfExpired()
    {

        var expired = DateTime.Now.ToUniversalTime().CompareTo(ExpiresAt);
        if (expired > 0)
        {
            throw new InvitationExpiredException();
        }
    }


    public static OrganizationInvite Create(Guid memberId, Guid organizationId, string token)
    {
        var expiresAt = new DateTime().AddDays(5).ToUniversalTime();
        if (token.Trim().Equals("")) throw new InvalidInvitationTokenException();
        return new OrganizationInvite(memberId, organizationId, token, expiresAt);
    }

    public static void ThrowIfDoesntExist(OrganizationInvite? invite)
    {
        if (invite is null) throw new OrganizationInviteNotFoundException();
    }
    
}