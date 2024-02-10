namespace Domain.Shared.Git.Payloads;

public class GitCommitContent
{
    public string sha { get; set; }
    public DateTime created { get; set; }
    public string html_url { get; set; }
    public Commit commit { get; set; }
    public Author author { get; set; }
    public Committer? committer { get; set; }
    public Parent[] parents { get; set; }
    public object files { get; set; }
    public Stats stats { get; set; }
}

public class Commit
{
    public string url { get; set; }
    public Author author { get; set; }
    public Committer committer { get; set; }
    public string message { get; set; }
    public Tree tree { get; set; }
    public object verification { get; set; }
}

public class Author
{
    public string name { get; set; }
    public string email { get; set; }
    public DateTime date { get; set; }
}

public class Committer
{
    public string name { get; set; }
    public string email { get; set; }
    public DateTime date { get; set; }
}

public class Tree
{
    public string url { get; set; }
    public string sha { get; set; }
    public DateTime created { get; set; }
}

public class Parent
{
    public string url { get; set; }
    public string sha { get; set; }
    public DateTime created { get; set; }
}

public class Stats
{
    public int total { get; set; }
    public int additions { get; set; }
    public int deletions { get; set; }
}