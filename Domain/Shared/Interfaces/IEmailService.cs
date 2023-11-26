namespace Domain.Shared.Interfaces;

public interface IEmailService
{
    Task SendOrgInvitationLink(string email, string link, string organizationName);
    
    Task SendRepoInvitationLink(string email, string link, string repositoryName);
}