using Application.Shared;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;

namespace Application.Organizations.Commands.Delete;

public class DeleteOrganizationCommandHandler: ICommandHandler<DeleteOrganizationCommand>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IPermissionService _permissionService;
    public DeleteOrganizationCommandHandler(IOrganizationMemberRepository organizationMemberRepository, IOrganizationRepository organizationRepository, IPermissionService permissionService)
    {
        _organizationMemberRepository = organizationMemberRepository;
        _organizationRepository = organizationRepository;
        _permissionService = permissionService;
    }

    public async Task Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        await _permissionService.ThrowIfNoPermission(
            new PermissionParams
            {
             Authorized   = request.UserId,
             OrganizationId = request.OrganizationId,
             Permission = "owner" 
            });
        
        Organization organization = _organizationRepository.Find(request.OrganizationId);
        _organizationRepository.Delete(organization);
    }
}