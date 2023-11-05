using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class PermissionDeniedException: BaseException
{
    public PermissionDeniedException() : base("No permission to access given resource!")
    {
    }
}