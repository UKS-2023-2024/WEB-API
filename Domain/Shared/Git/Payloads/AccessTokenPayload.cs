using System.Text.Json.Serialization;

namespace Domain.Shared.Git.Payloads;

public class AccessTokenPayload
{
    
    public int id { get; set; }

    public string name { get; set; }

    public string sha1 { get; set; }
    
    public string token_last_eighth { get; set; }
    
    public List<string>? scopes { get; set; }
}
