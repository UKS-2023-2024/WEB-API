using Domain.Repositories;
using System.Collections;
using WEB_API.Organization.Presenters;

namespace WEB_API.Repositories.Presenters;

public class RepositoryPresenter
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsPrivate { get; set; }
    public OrganizationPresenter Organization { get; set; }
    public IEnumerable<RepositoryMemberPresenter> Members { get; set; }
    
    public RepositoryPresenter(Repository repository)
    {
        Id = repository.Id;
        Name = repository.Name;
        Description = repository.Description;
        IsPrivate = repository.IsPrivate;
        if (repository.Organization != null) 
            Organization = new OrganizationPresenter(repository.Organization);
        Members = RepositoryMemberPresenter.MapRepositoryMembersToPresenters(repository.Members);
        
    }
    
    public static IEnumerable<RepositoryPresenter> MapRepositoriesToPresenters(IEnumerable<Repository> repositories)
    {
        return repositories.Select(org => new RepositoryPresenter(org)).ToList();
    }
}