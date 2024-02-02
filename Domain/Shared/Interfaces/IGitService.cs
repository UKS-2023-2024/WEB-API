using Domain.Auth;
using Domain.Branches;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Shared.Git.Payloads;

namespace Domain.Shared.Interfaces;

public interface IGitService
{
    public Task<string> CreateUser(User user, string password);
    public Task<GiteaRepoCreated?> CreatePersonalRepository(User user, Repository repository);
    public Task<GiteaRepoCreated?> CreateOrganizationRepository(User user,Organization organization, Repository repository);
    public Task<int> CreateOrganization(User user, Organization organization);
    public Task DeleteRepository(string owner, Repository repository);
    public Task DeleteOrganization(User user, Organization organization);
    public Task AddOrganizationMember(User user, Organization organization);
    public Task RemoveOrganizationMember(User user, Organization organization);
    public Task AddRepositoryMember(string owner, Repository repository, User user, string permission);
    public Task RemoveRepositoryMember(string owner, Repository repository, User user);
    public Task DeleteUser(User user);
    public Task SetPublicKey(User user, string pk);
    public Task DeleteBranch(User user, Branch branch);
    public Task<List<ContributionFile>> ListFolderContent(User user, Branch branch, string path);

}