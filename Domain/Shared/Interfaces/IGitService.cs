using Domain.Auth;
using Domain.Branches;
using Domain.Repositories;

namespace Domain.Shared.Interfaces;

public interface IGitService
{
    public Task<string> CreateUser(User user, string password);
    public Task<GiteaRepoCreated?> CreateRepository(User user, Repository repository);
    public Task DeleteRepository(User user, Repository repository);
    public Task SetPublicKey(User user, string pk);
    public Task DeleteBranch(User user, Branch branch);
    public Task<List<ContributionFile>> ListFolderContent(User user, Branch branch, string path);

}