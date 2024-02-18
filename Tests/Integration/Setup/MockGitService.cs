using Domain.Auth;
using Domain.Branches;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Tests.Integration.Setup;

public class MockGitService: IGitService
{
    public Task<string> CreateUser(User user, string password)
    {
        return Task.FromResult("");
    }

    public Task<GiteaRepoCreated?> CreatePersonalRepository(User user, Repository repository)
    {
        return Task.FromResult(new GiteaRepoCreated())!;
    }

    public Task<GiteaRepoCreated?> CreateOrganizationRepository(Organization organization, Repository repository)
    {
        return Task.FromResult(new GiteaRepoCreated())!;
    }

    public Task<int> CreateOrganization(User user, Organization organization)
    {
        return Task.FromResult(1);
    }

    public Task DeleteRepository(Repository repository)
    {
        return Task.CompletedTask;
    }

    public Task UpdateRepository(Repository repository, string defaultBranchName, string oldName)
    {
        return Task.CompletedTask;
    }

    public Task ForkRepository(Repository repository, User creator)
    {
        return Task.CompletedTask;
    }

    public Task DeleteOrganization(User user, Organization organization)
    {
        return Task.CompletedTask;
    }

    public Task AddOrganizationMember(User user, Organization organization)
    {
        return Task.CompletedTask;
    }

    public Task RemoveOrganizationMember(User user, Organization organization)
    {
        return Task.CompletedTask;
    }

    public Task AddRepositoryMember(Repository repository, User user, string permission)
    {
        return Task.CompletedTask;
    }

    public Task RemoveRepositoryMember(Repository repository, User user)
    {
        return Task.CompletedTask;
    }

    public Task MergePullRequest(Repository repository, string mergeType, int gitPullRequestId)
    {
        return Task.CompletedTask;
    }

    public Task UpdatePullRequest(Repository repository, int gitPullRequestId, string updateState)
    {
        return Task.CompletedTask;
    }

    public Task DeleteUser(User user)
    {
        return Task.CompletedTask;
    }

    public Task SetPublicKey(User user, string pk)
    {
        return Task.CompletedTask;
    }

    public Task DeleteBranch(User user, Branch branch)
    {
        return Task.CompletedTask;
    }

    public Task CreateBranch(Repository repository, string branchName, string createdFromBranch)
    {
        return Task.CompletedTask;
    }

    public Task<int> CreatePullRequest(Repository repository, string fromBranch, string toBranch, PullRequest pullRequest)
    {
        return Task.FromResult(1);
    }

    public Task<List<CommitContent>> ListBranchCommits(User user, Branch branch)
    {
        return Task.FromResult(new List<CommitContent>());
    }

    public Task<List<ContributionFile>> ListFolderContent(User user, Branch branch, string path)
    {
        return Task.FromResult(new List<ContributionFile>());
    }

    public Task<FileContent> ListFileContent(User user, Branch branch, string path)
    {
        return Task.FromResult(new FileContent());
    }

    public Task<string> GetPrDiffPreview(User user, Repository repository, PullRequest pullRequest)
    {
        return Task.FromResult("");
    }

    public Task<List<CommitContent>> ListPrCommits(User user, Repository repository, PullRequest pullRequest)
    {
        return Task.FromResult(new List<CommitContent>());
    }
}