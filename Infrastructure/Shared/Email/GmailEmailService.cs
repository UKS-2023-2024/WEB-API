using System.Net;
using System.Net.Mail;
using Domain.Auth;
using Domain.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Shared.Email;

public class GmailEmailService: IEmailService
{
    private readonly string _sender;
    private readonly string _key;
    
    public GmailEmailService(IConfiguration configuration)
    {
        _key = configuration["Email:Key"] ?? "";
        _sender = configuration["Email:Sender"] ?? "";
    }
    
    private MailMessage GenerateMailMessage(string email)
    {
        var newMail = new MailMessage();
        newMail.From = new MailAddress(_sender, "Githoob");
        newMail.To.Add(email);
        return newMail;
    }
    private void SendMail(MailMessage newMail)
    {
        newMail.IsBodyHtml = true;
        var client = new SmtpClient("smtp.gmail.com", 587);
        client.EnableSsl = true;
        NetworkCredential netCre = new NetworkCredential(_sender, _key);
        client.Credentials = netCre;
        client.Send(newMail);
    }
    public async Task SendOrgInvitationLink(string email, string link,string organizationName)
    {
        MailMessage newMail = GenerateMailMessage(email);
        newMail.Subject = "Invite for organization";
        newMail.Body = "<h1>You have been invited to organization : "+ organizationName +"!</h1> <br/>" +
                       "<h2>Accept invite <a href=\"" + link + "\">here<a/>!</h2> <br/>" +
                       "<h2><strong>Sent from Githoob org!</strong></h2>";
        SendMail(newMail);
    }

    public async Task SendRepoInvitationLink(string email, string link,string repositoryName)
    {
        MailMessage newMail = GenerateMailMessage(email);
        newMail.Subject = "Invite for repository";
        newMail.Body = "<h1>You have been invited to repository : "+ repositoryName +"!</h1> <br/>" +
                       "<h2>Accept invite <a href=\"" + link + "\">here<a/>!</h2> <br/>" +
                       "<h2><strong>Sent from Githoob org!</strong></h2>";
        SendMail(newMail);
    }

    public async Task SendNotificationIssueIsOpen(User user, Domain.Tasks.Issue issue, string repositoryName)
    {
        MailMessage newMail = GenerateMailMessage(user.PrimaryEmail);
        newMail.Subject = $"[Github] New Issue opened in {repositoryName}";
        newMail.Body = $"Hello {user.Username}! <br><br>" +
                        $"A new issue has been opened in the repository {repositoryName}<br><br>" +
                        $"Title: {issue.Title} <br>" +
                        $"Description: {issue.Description}<br>" +
                        $"Opened by: {issue.Creator?.Username}<br><br>" +
                        $"You are receiving this email because you are watching the repository. <br><br>" +
                        $"The GitHub Team";
        SendMail(newMail);
    }
}