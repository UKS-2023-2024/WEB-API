using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;

namespace Domain.Organizations.Services;

public class PermissionService: IPermissionService
{
    private IOrganizationRepository _organizationRepository;

    public PermissionService(IOrganizationRepository organizationRepository) =>
        _organizationRepository = organizationRepository;
    
    public async Task ThrowIfNoPermission(PermissionParams data)
    {
        var memberWithPermission = await _organizationRepository.FindMemberWithOrgPermission(data);
        if (memberWithPermission is null)
            throw new PermissionDeniedException();
    }
}