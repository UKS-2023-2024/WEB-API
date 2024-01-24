using Domain.Auth;
using Domain.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Shared.Email;

public class SendgridEmailService: IEmailService
{
    private readonly SendGridClient _sendgrid;
    private readonly string _sender;

    public SendgridEmailService(IConfiguration configuration)
    {
        _sendgrid = new SendGridClient(configuration["Sendgrid:ApiKey"]);
        _sender = configuration["Sendgrid:Sender"];
    }
    public async Task SendOrgInvitationLink(string email, string link, string organizationName)
    {
        EmailAddress from = new EmailAddress(_sender);
        EmailAddress to = new EmailAddress(email);
        const string subject = "You have been invited!";
        string plainTextContent = $"You have been invited to organization {link}";
        string htmlContent = "";
        SendGridMessage message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        await _sendgrid.SendEmailAsync(message);
    }

    public async Task SendRepoInvitationLink(string email, string link, string repositoryName)
    {
        EmailAddress from = new EmailAddress(_sender);
        EmailAddress to = new EmailAddress(email);
        const string subject = "You have been invited!";
        string plainTextContent = $"You have been invited to repository <a>{link}";
        string htmlContent = "";
        SendGridMessage message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        await _sendgrid.SendEmailAsync(message);
    }

    public async Task SendNotificationIssueIsOpen(User user, Domain.Tasks.Issue issue, string repositoryName)
    {
        EmailAddress from = new EmailAddress(_sender);
        EmailAddress to = new EmailAddress(user.PrimaryEmail);
        string subject = $"[Github] New Issue opened in {repositoryName}";
        string plainTextContent = $"Hello {user.Username} <br><br>" +
                                   $"A new issue has been opened in the repository {repositoryName}<br><br>" +
                                   $"Title: {issue.Title} <br>"  +
                                   $"Description: {issue.Description}<br>" +
                                   $"Opened by: {issue.Creator?.Username}<br><br>" +
                                   $"You are receiving this email because you are watching the repository. <br><br>" +
                                   $"The GitHub Team";
        string htmlContent = "";
        SendGridMessage message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        await _sendgrid.SendEmailAsync(message);
    }
}