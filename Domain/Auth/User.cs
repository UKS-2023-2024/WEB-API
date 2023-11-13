using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Organizations;
using Domain.Repositories;

namespace Domain.Auth;

public class User
{
    public Guid Id { get; private set; }
    public string PrimaryEmail { get; private set; }
    public string FullName { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public UserRole Role { get; private set; }
    public string? Bio { get; private set; }
    public string? Location { get; private set; }
    public string? Company { get; private set; }
    public string? Website { get; private set; }
    public List<SocialAccount>? SocialAccounts { get; private set; }
    public List<Email>? SecondaryEmails { get; private set; } = new();
    public bool Deleted { get; private set; }
    public List<Repository> PendingRepositories { get; private set; }
    public List<Repository> Starred { get; private set; }
    public List<OrganizationMember> Members { get; private set; }
    public List<OrganizationInvite> Invites { get; private set; }
    private User() { }
    private User(string primaryEmail, string fullName, string username, string password, UserRole role, string bio, string location, string company, string website, List<SocialAccount> socialAccounts, List<Email> secondaryEmails,List<Repository> starred)
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
        Deleted = false;
        Starred = starred;
    }

    private User(string primaryEmail, string fullName, string username, string password, UserRole role)
    {
        PrimaryEmail = primaryEmail;
        FullName = fullName;
        Username = username;
        Password = password;
        Role = role;
        SecondaryEmails = new();
        SocialAccounts = new();
        Deleted = false;
    }

    public static User Create(string primaryEmail, string fullName,string username, string password, UserRole role, string bio, string location, string company, string website, List<SocialAccount> socialAccounts, List<Email> secondaryEmails, List<Repository> starred)
    {
        if (primaryEmail == null) throw new Exception("Primary email cannot be null");
        if (username == null) throw new Exception("Username cannot be null");
        return new User(primaryEmail, fullName, username, password, role, bio, location, company, website, socialAccounts, secondaryEmails, starred);
    }

    public static User Create(string primaryEmail, string fullName, string username, string password, UserRole role)
    {
        if (primaryEmail == null) throw new Exception("Primary email cannot be null");
        if (username == null) throw new Exception("Username cannot be null");
        return new User(primaryEmail, fullName, username, password, role);
    }

    public static User Create(Guid id, string primaryEmail, string fullName, string username, string password, UserRole role)
    {
        if (primaryEmail == null) throw new Exception("Primary email cannot be null");
        if (username == null) throw new Exception("Username cannot be null");
        User user = new User(primaryEmail, fullName, username, password, role);
        user.Id = id;
        return user;
    }

    public void Delete()
    {
        Deleted = true;
    }

    public void Update(string fullName, string bio, string company, string location, string website, List<SocialAccount> socialAccounts)
    {
        FullName = fullName;   
        Bio = bio;
        Company = company;
        Location = location;
        Website = website;
        SocialAccounts = socialAccounts;
    }

    public static void ThrowIfDoesntExist(User? user)
    {
        if (user is null) throw new UserNotFoundException();
    }
    
}