using Application.Shared;
using Domain.Organizations;
using Domain.Organizations.Interfaces;

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
        OrganizationMember? member = await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.userId, request.organizationId);
        if (member is null || member.Role != OrganizationMemberRole.OWNER)
            throw new UnauthorizedAccessException();
        Organization organization = _organizationRepository.Find(request.organizationId);
        _organizationRepository.Delete(organization);
    }
}