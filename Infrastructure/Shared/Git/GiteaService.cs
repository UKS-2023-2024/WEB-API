using System.Drawing.Printing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Domain.Auth;
using Domain.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Shared.Git;

public class GiteaService: IGitService
{
    private HttpClient _httpClient;
    private string _adminUsername;
    private string _adminPassword;
    private string _adminToken;
    
    public GiteaService(IConfiguration configuration)
    {
        var giteaBaseUrl = configuration["Gitea:BaseUrl"] ?? "";
        _adminUsername = configuration["Gitea:AdminUsername"] ?? "";
        _adminPassword = configuration["Gitea:AdminPassword"] ?? "";
        _adminToken = configuration["Gitea:AdminToken"] ?? "";
        
        _httpClient = new()
        {
            BaseAddress = new Uri(giteaBaseUrl)
        };
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", _adminToken);
    }
    
    
    public async Task CreateUser(User user, string password)
    {
        var url = "admin/users";

        var body = Body(new
        {
            email = user.PrimaryEmail,
            username = user.Username,
            password
        });
        
        var response = await _httpClient.PostAsync(url, body);
        if (response.StatusCode != HttpStatusCode.Created)
        {
            await LogStatusAndResponseContent(response);
        }
    }

    public async Task SetPublicKey(User user, string key)
    {
        var url = $"admin/users/{user.Username}/keys";
        var body = Body(new
        {
            key,
            read_only = false,
            title = "main_pk"
        });

        var response = await _httpClient.PostAsync(url, body);
        await LogStatusAndResponseContent(response);
    }

    private async Task LogStatusAndResponseContent(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"HTTP REQUEST FAILED - MESSAGE: {content} - STATUS_CODE: {response.StatusCode}");
    }
    private StringContent Body(object data)
    {
        var jsonData = JsonSerializer.Serialize(data);
        return new StringContent(jsonData, Encoding.UTF8, "application/json");
    }
    
    
    
    
    

}