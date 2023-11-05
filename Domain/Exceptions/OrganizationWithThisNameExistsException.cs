namespace Domain.Exceptions;

public class OrganizationWithThisNameExistsException : BaseException
{
    public OrganizationWithThisNameExistsException() : base("Organization with this name already exists!")
    {
    }
}