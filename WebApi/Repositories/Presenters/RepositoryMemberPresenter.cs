using Domain.Repositories;
using WEB_API.Organization.Presenters;

namespace WEB_API.Repositories.Presenters;

public class RepositoryMemberPresenter
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public RepositoryMemberRole Role { get; set; }
    
    public RepositoryMemberPresenter(RepositoryMember repositoryMember)
    {
        Username = repositoryMember.Member.Username;
        Id = repositoryMember.Id;
        Role = repositoryMember.Role;
    }
    
    public static IEnumerable<RepositoryMemberPresenter> MapRepositoryMembersToPresenters(
        IEnumerable<RepositoryMember> repositoryMembers)
    {
        return repositoryMembers.Select(org => new RepositoryMemberPresenter(org)).ToList();
    }
}