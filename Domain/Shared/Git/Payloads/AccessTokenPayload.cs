using System.Text.Json.Serialization;

namespace Domain.Shared.Git.Payloads;

public class AccessTokenPayload
{
    
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("sha1")]
    public string Sha1 { get; set; }
    
    [JsonPropertyName("token_last_eight")]
    public string TokenLastEight { get; set; }
    
    [JsonPropertyName("scopes")]
    public List<string>? Scopes { get; set; }
}
