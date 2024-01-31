using System.Net.Http.Headers;
using System.Text;
using Domain.Auth;
using Domain.Branches;
using Domain.Repositories;
using Domain.Shared.Git.Payloads;
using Domain.Shared.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Infrastructure.Shared.Git;

public class GiteaService: IGitService
{
    private HttpClient _httpClient;
    private string _adminToken;

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
        "write:user"
    };
    
    public GiteaService(IConfiguration configuration)
    {
        var giteaBaseUrl = configuration["Gitea:BaseUrl"] ?? "";
        _adminToken = configuration["Gitea:AdminToken"] ?? "";
        
        _httpClient = new()
        {
            BaseAddress = new Uri(giteaBaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        SetAuthTokenHeader(_adminToken);
    }
    
    
    public async Task<string> CreateUser(User user, string password)
    {
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
        var base64Credentials = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes($"{user.Username}:{plainPassword}")
        );
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", base64Credentials);
        
        var body = Body(new
        {
            name = "uks_access_token",
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

    public async Task SetPublicKey(User user, string key)
    {
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

    public async Task<GiteaRepoCreated?> CreateRepository(User user, Repository repository)
    {
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
        return await DeserializeBody<GiteaRepoCreated>(response);
    }
    public async Task DeleteRepository(User user, Repository repository)
    {
        var url = $"repos/{user.Username}/{repository.Name}";
        SetAuthTokenHeader(user.GitToken);
        var response = await _httpClient.DeleteAsync(url);
        await LogStatusAndResponseContent(response);
        SetAuthTokenHeader(_adminToken);
    }

    public async Task DeleteBranch(User user, Branch branch)
    {
        var url = $"repos/{user.Username}/{branch.Repository.Name}/branches/{branch.Name}";
        var response = await _httpClient.DeleteAsync(url);
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

    private void SetAuthTokenHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);
    }
    
    
    
    
    

}