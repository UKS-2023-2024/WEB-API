using Domain.Auth;

namespace Domain.Shared.Interfaces;

public interface IEmailService
{
    Task SendOrgInvitationLink(string email, string link, string organizationName);
    
    Task SendRepoInvitationLink(string email, string link, string repositoryName);
    Task SendNotificationIssueIsOpen(User user, Tasks.Issue issue, string repositoryName);
}