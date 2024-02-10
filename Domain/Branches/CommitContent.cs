namespace Domain.Branches;

public class CommitContent
{
    public string Sha { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Message { get; set; }
    public string? Committer { get; set; }
    public int Additions { get; set; }
    public int Deletions { get; set; }
}