namespace WEB_API.Search.Presenter;

public class QueryPayload
{
    private Guid Id { get; set; }
    private string? Name { get; set; }
    private string? Username { get; set; }
    private string? Type { get; set; }

    public QueryPayload()
    {
    }

    public QueryPayload(Guid id, string? name, string? username, string? type)
    {
        Id = id;
        Name = name;
        Username = username;
        Type = type;
    }
}