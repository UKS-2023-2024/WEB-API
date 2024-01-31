namespace Domain.Shared.Git.Payloads;

using System;

public class GitCommitAuthor
{
    public string name { get; set; }
    public string email { get; set; }
    public string username { get; set; }
}

public class GitCommitCommitter
{
    public string name { get; set; }
    public string email { get; set; }
    public string username { get; set; }
}

public class GitCommitVerification
{
    public bool verified { get; set; }
    public string reason { get; set; }
    public string signature { get; set; }
    public object signer { get; set; }
    public string payload { get; set; }
}

public class GitCommit
{
    public string id { get; set; }
    public string message { get; set; }
    public string url { get; set; }
    public GitCommitAuthor author { get; set; }
    public GitCommitCommitter committer { get; set; }
    public GitCommitVerification verification { get; set; }
    public DateTime timestamp { get; set; }
    public object added { get; set; }
    public object removed { get; set; }
    public object modified { get; set; }
}

public class GitBranch
{
    public string name { get; set; }
    public GitCommit commit { get; set; }
    public bool @protected { get; set; }
    public int required_approvals { get; set; }
    public bool enable_status_check { get; set; }
    public string[] status_check_contexts { get; set; }
    public bool user_can_push { get; set; }
    public bool user_can_merge { get; set; }
    public string effective_branch_protection_name { get; set; }
}