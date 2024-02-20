using System.Text.Json;

namespace Domain.Shared.Git.Payloads;

public class CreateTeamOption
{
    public string description { get; set; }
    public bool can_create_org_repo { get; set; }
    public string includes_all_repositories { get; set; }
    public string name { get; set; }
    
    public int id { get; set; }
    public string permission { get; set; }
    public List<string> units { get; set; }
    public Dictionary<string, string> units_map { get; set; }
    
    public CreateTeamOption()
    {
        description = "Members team";
        can_create_org_repo = true;
        includes_all_repositories = "all";
        name = "members";
        permission = "read";
        units = new List<string>
        {
            "repo.actions",
            "repo.code",
            "repo.issues",
            "repo.ext_issues",
            "repo.wiki",
            "repo.ext_wiki",
            "repo.pulls",
            "repo.releases",
            "repo.projects",
            "repo.ext_wiki"
        };
        units_map = new Dictionary<string, string>
        {
            { "repo.actions", "write" },
            { "repo.packages", "write" },
            { "repo.code", "write" },
            { "repo.issues", "write" },
            { "repo.ext_issues", "write" },
            { "repo.wiki", "write" },
            { "repo.pulls", "write" },
            { "repo.releases", "write" },
            { "repo.projects", "write" },
            { "repo.ext_wiki", "write" }
        };
        units_map = new Dictionary<string, string>();
        SetupUnitsMapDefaults();
    }
    
    private void SetupUnitsMapDefaults()
    {
        // Set default values for UnitsMap
        foreach (var unit in units)
        {
            units_map[unit] = "2"; // Default value of "2"
        }

        // Assign specific values from your mapping
        units_map["unit_6"] = "1";
        units_map["unit_7"] = "1";
    }

    public string SerializeToJson()
    {
        return JsonSerializer.Serialize(this);
    }
}