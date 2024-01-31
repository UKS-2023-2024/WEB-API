using System.Text.Json.Serialization;

public class Owner
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("login")]
    public string Login { get; set; }

    [JsonPropertyName("login_name")]
    public string LoginName { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("is_admin")]
    public bool IsAdmin { get; set; }

    [JsonPropertyName("last_login")]
    public DateTime LastLogin { get; set; }

    [JsonPropertyName("created")]
    public DateTime Created { get; set; }

    [JsonPropertyName("restricted")]
    public bool Restricted { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("prohibit_login")]
    public bool ProhibitLogin { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("website")]
    public string Website { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; }

    [JsonPropertyName("followers_count")]
    public int FollowersCount { get; set; }

    [JsonPropertyName("following_count")]
    public int FollowingCount { get; set; }

    [JsonPropertyName("starred_repos_count")]
    public int StarredReposCount { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }
}

public class Permissions
{
    [JsonPropertyName("admin")]
    public bool Admin { get; set; }

    [JsonPropertyName("push")]
    public bool Push { get; set; }

    [JsonPropertyName("pull")]
    public bool Pull { get; set; }
}

public class InternalTracker
{
    [JsonPropertyName("enable_time_tracker")]
    public bool EnableTimeTracker { get; set; }

    [JsonPropertyName("allow_only_contributors_to_track_time")]
    public bool AllowOnlyContributorsToTrackTime { get; set; }

    [JsonPropertyName("enable_issue_dependencies")]
    public bool EnableIssueDependencies { get; set; }
}

public class GiteaRepoCreated
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("owner")]
    public Owner Owner { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("empty")]
    public bool Empty { get; set; }

    [JsonPropertyName("private")]
    public bool Private { get; set; }

    [JsonPropertyName("fork")]
    public bool Fork { get; set; }

    [JsonPropertyName("template")]
    public bool Template { get; set; }

    [JsonPropertyName("parent")]
    public object Parent { get; set; }

    [JsonPropertyName("mirror")]
    public bool Mirror { get; set; }

    [JsonPropertyName("size")]
    public int Size { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("languages_url")]
    public string LanguagesUrl { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("ssh_url")]
    public string SshUrl { get; set; }

    [JsonPropertyName("clone_url")]
    public string CloneUrl { get; set; }

    [JsonPropertyName("original_url")]
    public string OriginalUrl { get; set; }

    [JsonPropertyName("website")]
    public string Website { get; set; }

    [JsonPropertyName("stars_count")]
    public int StarsCount { get; set; }

    [JsonPropertyName("forks_count")]
    public int ForksCount { get; set; }

    [JsonPropertyName("watchers_count")]
    public int WatchersCount { get; set; }

    [JsonPropertyName("open_issues_count")]
    public int OpenIssuesCount { get; set; }

    [JsonPropertyName("open_pr_counter")]
    public int OpenPrCounter { get; set; }

    [JsonPropertyName("release_counter")]
    public int ReleaseCounter { get; set; }

    [JsonPropertyName("default_branch")]
    public string DefaultBranch { get; set; }

    [JsonPropertyName("archived")]
    public bool Archived { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("archived_at")]
    public DateTime ArchivedAt { get; set; }

    [JsonPropertyName("permissions")]
    public Permissions Permissions { get; set; }

    [JsonPropertyName("has_issues")]
    public bool HasIssues { get; set; }

    [JsonPropertyName("internal_tracker")]
    public InternalTracker InternalTracker { get; set; }

    [JsonPropertyName("has_wiki")]
    public bool HasWiki { get; set; }

    [JsonPropertyName("has_pull_requests")]
    public bool HasPullRequests { get; set; }

    [JsonPropertyName("has_projects")]
    public bool HasProjects { get; set; }

    [JsonPropertyName("has_releases")]
    public bool HasReleases { get; set; }

    [JsonPropertyName("has_packages")]
    public bool HasPackages { get; set; }

    [JsonPropertyName("has_actions")]
    public bool HasActions { get; set; }

    [JsonPropertyName("ignore_whitespace_conflicts")]
    public bool IgnoreWhitespaceConflicts { get; set; }

    [JsonPropertyName("allow_merge_commits")]
    public bool AllowMergeCommits { get; set; }

    [JsonPropertyName("allow_rebase")]
    public bool AllowRebase { get; set; }

    [JsonPropertyName("allow_rebase_explicit")]
    public bool AllowRebaseExplicit { get; set; }

    [JsonPropertyName("allow_squash_merge")]
    public bool AllowSquashMerge { get; set; }

    [JsonPropertyName("allow_rebase_update")]
    public bool AllowRebaseUpdate { get; set; }

    [JsonPropertyName("default_delete_branch_after_merge")]
    public bool DefaultDeleteBranchAfterMerge { get; set; }

    [JsonPropertyName("default_merge_style")]
    public string DefaultMergeStyle { get; set; }

    [JsonPropertyName("default_allow_maintainer_edit")]
    public bool DefaultAllowMaintainerEdit { get; set; }

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonPropertyName("internal")]
    public bool Internal { get; set; }

    [JsonPropertyName("mirror_interval")]
    public string MirrorInterval { get; set; }

    [JsonPropertyName("mirror_updated")]
    public DateTime MirrorUpdated { get; set; }

    [JsonPropertyName("repo_transfer")]
    public object RepoTransfer { get; set; }
}