namespace Domain.Branches;

public class FileContent
{
    public string Content { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Encoding { get; set; }

    public FileContent(string content, string name, string path, string encoding)
    {
        Content = content;
        Name = name;
        Path = path;
        Encoding = encoding;
    }

    public FileContent()
    {
    }
}