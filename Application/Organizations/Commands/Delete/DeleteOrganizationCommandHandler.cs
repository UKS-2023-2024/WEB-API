using Application.Shared;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;

namespace Application.Organizations.Commands.Delete;

public class DeleteOrganizationCommandHandler: ICommandHandler<DeleteOrganizationCommand>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly IOrganizationRepository _organizationRepository;
    public DeleteOrganizationCommandHandler(IOrganizationMemberRepository organizationMemberRepository, IOrganizationRepository organizationRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        var sender =
            await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.UserId, request.OrganizationId);
        sender.ThrowIfNoAdminPrivileges();
        
        var organization = _organizationRepository.Find(request.OrganizationId);
        _organizationRepository.Delete(organization!);
    }
}