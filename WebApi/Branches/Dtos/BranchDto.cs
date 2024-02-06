namespace WEB_API.Branches.Dtos
{
    public class BranchDto
    {
        required public string Name { get; set; }
        required public Guid RepositoryId { get; set; }

        required public string CreatedFrom { get; set; }

    }
}
