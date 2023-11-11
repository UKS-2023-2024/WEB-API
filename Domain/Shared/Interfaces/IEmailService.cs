namespace Application.Shared.Email;

public interface IEmailService
{
    Task SendOrgInvitationLink(string email, string link);
}