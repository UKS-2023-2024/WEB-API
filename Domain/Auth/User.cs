using Domain.Auth.Enums;

namespace Domain.Auth;

public class User
{
    public Guid Id { get; private set; }
    public string PrimaryEmail { get; private set; }
    public string FullName { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public UserRole Role { get; private set; }
    public string Bio { get; private set; }
    public string Location { get; private set; }
    public string Company { get; private set; }
    public string Website { get; private set; }
    public List<SocialAccount> SocialAccounts { get; private set; }
    public List<Email> SecondaryEmails { get; private set; }

    private User() { }
    private User(string primaryEmail, string fullName, string username, string password, UserRole role, string bio, string location, string company, string website, List<SocialAccount> socialAccounts, List<Email> secondaryEmails)
    {
        PrimaryEmail = primaryEmail;
        FullName = fullName;
        Username = username;
        Role = role;
        Password = password;
        Bio = bio;
        Location = location;
        Company = company;
        Website = website;
        SocialAccounts = socialAccounts;
        SecondaryEmails = secondaryEmails;
    }

    public static User Create(string primaryEmail, string fullName,string username, string password, UserRole role, string bio, string location, string company, string website, List<SocialAccount> socialAccounts, List<Email> secondaryEmails)
    {
        if (primaryEmail == null) throw new Exception("Primary email cannot be null");
        if (username == null) throw new Exception("Username cannot be null");
        return new User(primaryEmail, fullName, username, password, role, bio, location, company, website, socialAccounts, secondaryEmails);
    }



}