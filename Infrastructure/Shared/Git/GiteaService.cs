using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Domain.Auth;
using Domain.Branches;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Shared.Exceptions;
using Domain.Shared.Git.Payloads;
using Domain.Shared.Interfaces;
using Domain.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Shared.Git;

public class GiteaService: IGitService
{
    private HttpClient _httpClient;
    private string _adminUsername;
    private string _adminPassword;

    private readonly List<string> ALL_SCOPES = new()
    {
        "read:issue",
        "write:issue",
        "read:notification",
        "write:notification",
        "read:organization",
        "write:organization",
        "read:repository",
        "write:repository",
        "read:user",
        "write:user",
        "read:admin",
        "write:admin"
    };

    public GiteaService(IConfiguration configuration)
    {
        var giteaBaseUrl = configuration["Gitea:BaseUrl"] ?? "";
        _adminUsername = configuration["Gitea:AdminUsername"] ?? "";
        _adminPassword= configuration["Gitea:AdminPassword"] ?? "";
        
        _httpClient = new()
        {
            BaseAddress = new Uri(giteaBaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> CreateUser(User user, string password)
    {
        SetAdminBasicAuthToken();
        var url = "admin/users";

        var body = Body(new
        {
            email = user.PrimaryEmail,
            username = user.Username,
            password,
            must_change_password = false
        });
        
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
        return await GenerateAccessToken(user, password);
    }

    public async Task<string> GenerateAccessToken(User user, string plainPassword)
    {
        var url = $"users/{user.Username}/tokens";
        SetBasicAuthToken(user.Username, plainPassword);
       
        var body = Body(new
        {
            name = new Guid().ToString(),
            scopes = ALL_SCOPES
        });
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
        AccessTokenPayload? obj = await DeserializeBody<AccessTokenPayload>(response);
        if (obj is not null)
        {
            Console.WriteLine(obj.sha1);
            return obj.sha1;
        }
        return "";

    }

    public async Task MergePullRequest(Repository repository, string mergeType,int gitPullRequestId)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/pulls/{gitPullRequestId}/merge";
        var body = Body(new
        {
            Do = mergeType
        });
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
    }
    
    public async Task UpdatePullRequest(Repository repository, int gitPullRequestId, string updateState)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/pulls/{gitPullRequestId}";
        var body = Body(new
        {
            state = updateState
        });
        var response = await _httpClient.PatchAsync(url, body);
        await LogStatusAndResponseContent(response);
    }

    public async Task DeleteUser(User user)
    {
        SetAdminBasicAuthToken();
        var url = $"admin/users/{user.Username}";
        var response = await _httpClient.DeleteAsync(url);
        await LogStatusAndResponseContent(response);
    }

    public async Task SetPublicKey(User user, string key)
    {
        SetAdminBasicAuthToken();
        var url = $"admin/users/{user.Username}/keys";
        var body = Body(new
        {
            key,
            read_only = false,
            title = "main_pk2"
        });

        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
    }

    public async Task<GiteaRepoCreated?> CreatePersonalRepository(User user, Repository repository)
    {
        SetAdminBasicAuthToken();
        var url = $"admin/users/{user.Username}/repos";
        var body = Body(new
        {
            name = repository.Name,
            default_branch = "main",
            @private = repository.IsPrivate,
            description = repository.Description,
        });
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
        await CreatePushWebhook(repository);
        return await DeserializeBody<GiteaRepoCreated>(response);
    }
    
    public async Task UpdateRepository(Repository repository,string defaultBranchName,string oldName)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{oldName}";
        var body = Body(new
        {
            name = repository.Name,
            default_branch = defaultBranchName,
            @private = repository.IsPrivate,
            description = repository.Description,
        });
        var response = await _httpClient.PatchAsync(url, body);
        await LogStatusAndResponseContent(response);
    }
    
    public async Task<GiteaRepoCreated?> CreateOrganizationRepository(Organization organization,Repository repository)
    {
        SetAdminBasicAuthToken();
        var url = $"orgs/{organization.Name}/repos";
        var body = Body(new
        {
            name = repository.Name,
            default_branch = "main",
            @private = repository.IsPrivate,
            description = repository.Description,
        });
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
        await CreatePushWebhook(repository);
        return await DeserializeBody<GiteaRepoCreated>(response);
    }
    public async Task RemoveRepositoryMember(Repository repository, User user)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/collaborators/{user.Username}";
        var response = await _httpClient.DeleteAsync(url);
        await LogStatusAndResponseContent(response);
    }
    public async Task DeleteBranch(Repository repository, Branch branch)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{branch.Repository.Name}/branches/{branch.OriginalName}";
        var response = await _httpClient.DeleteAsync(url);
        await LogStatusAndResponseContent(response);
    }

    public async Task CreateBranch(Repository repository, string branchName, string createdFromBranch)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/branches/";
        var body = Body(new
        {
            new_branch_name = branchName,
            old_branch_name = createdFromBranch,
            old_ref_name = createdFromBranch,
        });
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
    }

    public async Task<int> CreatePullRequest(Repository repository, string fromBranch, string toBranch, PullRequest pullRequest)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/pulls";
        var body = Body(new
        {
            head = fromBranch,
            @base = toBranch,
            title = pullRequest.Title,
            body = pullRequest.Description
        });
        var response = await _httpClient.PostAsync(url, body);
        var createdPullRequest = await DeserializeBody<GitCreatePrPayload>(response);
        await LogStatusAndResponseContent(response);
        if (createdPullRequest is null)
            throw new GitException("Problem creating pull request!");
        return createdPullRequest.number;
    }

    public async Task<List<CommitContent>> ListBranchCommits(Repository repository, Branch branch)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{branch.Repository.Name}/commits";
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["sha"] = branch.OriginalName;
        query["stat"] = "true";
        query["verification"] = "false";
        query["files"] = "true";
        var fullUrl = $"{url}?{query}";
        var response = await _httpClient.GetAsync(fullUrl);

        var commits = await DeserializeBody<List<GitCommitContent>>(response);
        if (commits is null) commits = new List<GitCommitContent>();
        return commits.Select(c => new CommitContent()
        {
            Additions = c.stats.additions,
            Deletions = c.stats.deletions,
            Message = c.commit.message,
            Sha = c.sha,
            CreatedAt = c.created,
            Committer = c.commit.committer?.name
        }).ToList();
    }

    public async Task<int> CreateOrganization(User user, Organization organization)
    {
        SetAuthToken(user.GitToken!);
        var url = $"orgs";
        var body = Body(new
        {
            email = organization.ContactEmail,
            full_name = organization.Name,
            location = organization.Location,
            description = organization.Description,
            username = organization.Name,
            visibility = "public",
            website = "",
            repo_admin_change_team_access = true
        });
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);

        return await CreateTeam(organization.Name);
    }

    private async Task<int> CreateTeam(string organizationName)
    {
        var url = $"orgs/{organizationName}/teams";
        var createTeamOption = new CreateTeamOption();

        var body = Body(new
        {
            description = createTeamOption.description,
            can_create_org_repo = createTeamOption.can_create_org_repo,
            includes_all_repositories = true,
            name = "members",
            permission = "admin",
            units = createTeamOption.units,
            units_map = createTeamOption.units_map
        });
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
        var team = await DeserializeBody<CreateTeamOption>(response);
        if (team is null)
            throw new GitException("Problem creating initial team!");
        return ((await DeserializeBody<CreateTeamOption>(response))!).id;
    }

    public async Task DeleteRepository(Repository repository)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}";
        var response = await _httpClient.DeleteAsync(url);
        await LogStatusAndResponseContent(response);
    }

    public async Task ForkRepository(Repository repository, User creator)
    {
        SetAuthToken(creator.GitToken!);
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/forks";
        var body = Body(new
        {
            name = repository.Name
        });
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
    }

    public async Task DeleteOrganization(User user, Organization organization)
    {
        SetAdminBasicAuthToken();
        var url = $"orgs/{organization.Name}";
        var response = await _httpClient.DeleteAsync(url);
        await LogStatusAndResponseContent(response);
    }
    
    public async Task AddOrganizationMember(User user, Organization organization)
    {
        SetAdminBasicAuthToken();
        var url = $"teams/{organization.memberTeamId}/members/{user.Username}";
        var response = await _httpClient.PutAsync(url,null);
        await LogStatusAndResponseContent(response);
    }
    
    public async Task RemoveOrganizationMember(User user, Organization organization)
    {
        SetAdminBasicAuthToken();
        var url = $"teams/{organization.memberTeamId}/members/{user.Username}";
        var response = await _httpClient.DeleteAsync(url);
        await LogStatusAndResponseContent(response);
    }
    
    public async Task AddRepositoryMember(Repository repository, User user, string permission)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/collaborators/{user.Username}";
        var body = Body(new
        {
            permission,
        });
        var response = await _httpClient.PutAsync(url,body);
        await LogStatusAndResponseContent(response);
    }

    public async Task<List<ContributionFile>> ListFolderContent(Repository repository, Branch branch, string path)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{branch.Repository.Name}/contents";
        var pathBasedUrl = path.Equals("/") ? url : url + $"{path}";
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["ref"] = branch.OriginalName;
        var fullUrl = $"{pathBasedUrl}?{query}";
        var response = await _httpClient.GetAsync(fullUrl);
        await LogStatusAndResponseContent(response);
        List<FileInformation> files = await DeserializeBody<List<FileInformation>>(response) ?? new();
        return files
            .Select(f => new ContributionFile(f.name, f.type.Equals("dir"), f.path))
            .ToList();
    }

    public async Task<FileContent> ListFileContent(Repository repository, Branch branch, string path)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{branch.Repository.Name}/contents/{path}";
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["ref"] = branch.OriginalName;
        var fullUrl = $"{url}?{query}";
        var response = await _httpClient.GetAsync(fullUrl);
        await LogStatusAndResponseContent(response);
        var fileContent = await DeserializeBody<GitFileContent>(response);
        if (fileContent is null) return null;
        return new FileContent(fileContent.content, fileContent.name, fileContent.path,  fileContent.encoding);        
    }

    public async Task<string> GetPrDiffPreview(User user, Repository repository, PullRequest pullRequest)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/pulls/{pullRequest.GitPullRequestId}.diff";
        var response = await _httpClient.GetAsync(url);
        await LogStatusAndResponseContent(response);
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<List<CommitContent>> ListPrCommits(User user, Repository repository, PullRequest pullRequest)
    {
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/pulls/{pullRequest.GitPullRequestId}/commits";
        var response = await _httpClient.GetAsync(url);
        await LogStatusAndResponseContent(response);
        var commits = await DeserializeBody<List<GitCommitContent>>(response);
        if (response.StatusCode != HttpStatusCode.OK) return new();
        if (commits is null) commits = new List<GitCommitContent>();
        return commits.Select(c => new CommitContent()
        {
            Additions = c.stats.additions,
            Deletions = c.stats.deletions,
            Message = c.commit.message,
            Sha = c.sha,
            CreatedAt = c.created,
            Committer = c.commit.committer?.name
        }).ToList();
    }

    private async Task CreatePushWebhook(Repository repository)
    {   
        SetAdminBasicAuthToken();
        var url = $"repos/{repository.FindRepositoryOwner()}/{repository.Name}/hooks";
        var body = Body(new
        {
            active = true,
            branch_filter = "*",
            config = new
            {
                url = "http://uks_backend/webhook",
                content_type = "json"
            },
            type = "gitea"
        });
        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
    }

    private async Task<T?> DeserializeBody<T>(HttpResponseMessage response)
    {
        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(result);
    }

    private async Task LogStatusAndResponseContent(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"HTTP REQUEST - MESSAGE: {content} - STATUS_CODE: {response.StatusCode}");
    }
    private StringContent Body(object data)
    {
        var jsonData = JsonSerializer.Serialize(data);
        return new StringContent(jsonData, Encoding.UTF8, "application/json");
    }

    private void SetAdminBasicAuthToken()
    {
        Console.WriteLine(_adminUsername, _adminPassword);
        SetBasicAuthToken(_adminUsername, _adminPassword);
    }

    private void SetBasicAuthToken(string username, string password)
    {
        var base64Credentials = GenerateCredentialsBase64(username, password);
        Console.WriteLine("Credentials for " + username + " password " + password + " " + base64Credentials);
        _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Basic", base64Credentials);
    }
    
    private void SetAuthToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);
    }

    private string GenerateCredentialsBase64(string username, string password)
    {
        var base64Credentials = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes($"{username}:{password}")
        );
        return base64Credentials;
    }

}