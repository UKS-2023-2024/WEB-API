namespace Domain.Shared.Git.Payloads;

public class FileInformation
{
    public string name { get; set; }
    public string path { get; set; }
    public string sha { get; set; }
    public string last_commit_sha { get; set; }
    public string type { get; set; }
    public int size { get; set; }
    public string encoding { get; set; }
    public object content { get; set; }
    public object target { get; set; }
    public string url { get; set; }
    public string html_url { get; set; }
    public string git_url { get; set; }
    public string download_url { get; set; }
    public object submodule_git_url { get; set; }
}