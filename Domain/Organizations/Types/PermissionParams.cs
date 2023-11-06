namespace Domain.Organizations.Types;

public class PermissionParams
{
    
    public Guid Authorized { get; set; }
    public Guid OrganizationId { get; set; }
    public string Permission { get; set; }
    public PermissionParams() {}
} 
