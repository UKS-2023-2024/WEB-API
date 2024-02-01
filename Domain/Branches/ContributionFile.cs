namespace Domain.Branches;

public class ContributionFile
{
    public string Name { get; set; }
    public string? Path { get; set; }
    public bool IsFolder { get; set; }

    public ContributionFile(string name, bool isFolder, string? path)
    {
        Name = name;
        IsFolder = isFolder;
        Path = path;
    } 
    
}