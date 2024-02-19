using Domain.Auth;
using Domain.Branches;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Shared.Git.Payloads;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Task = System.Threading.Tasks.Task;

namespace Domain.Shared.Interfaces;

public interface IGitService
{
    public Task<string> CreateUser(User user, string password);
    public Task<GiteaRepoCreated?> CreatePersonalRepository(User user, Repository repository);
    public Task<GiteaRepoCreated?> CreateOrganizationRepository(Organization organization, Repository repository);
    public Task<int> CreateOrganization(User user, Organization organization);
    public Task DeleteRepository(Repository repository);
    public Task UpdateRepository(Repository repository, string defaultBranchName,string oldName);
    public Task ForkRepository(Repository repository, User creator);
    public Task DeleteOrganization(User user, Organization organization);
    public Task AddOrganizationMember(User user, Organization organization);
    public Task RemoveOrganizationMember(User user, Organization organization);
    public Task AddRepositoryMember(Repository repository, User user, string permission);
    public Task RemoveRepositoryMember(Repository repository, User user);
    public Task MergePullRequest(Repository repository, string mergeType,int gitPullRequestId);
    public Task UpdatePullRequest(Repository repository,int gitPullRequestId, string updateState);
    public Task DeleteUser(User user);
    public Task SetPublicKey(User user, string pk);
    public Task DeleteBranch(Repository repository, Branch branch);
    public Task CreateBranch(Repository repository, string branchName, string createdFromBranch);
    public Task<int> CreatePullRequest(Repository repository, string fromBranch, string toBranch, PullRequest pullRequest);
    public Task<List<CommitContent>> ListBranchCommits(Repository repository, Branch branch);
    public Task<List<ContributionFile>> ListFolderContent(Repository repository, Branch branch, string path);
    public Task<FileContent> ListFileContent(Repository repository, Branch branch, string path);
    public Task<string> GetPrDiffPreview(User user, Repository repository, PullRequest pullRequest);
    public Task<List<CommitContent>> ListPrCommits(User user, Repository repository, PullRequest pullRequest);

}