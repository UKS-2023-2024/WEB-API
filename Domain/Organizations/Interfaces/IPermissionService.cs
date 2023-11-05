using Domain.Organizations.Types;

namespace Domain.Organizations.Interfaces;

public interface IPermissionService
{
    Task ThrowIfNoPermission(PermissionParams data);
}