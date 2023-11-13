using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Interfaces;

namespace Application.Organizations.Commands.AcceptInvite;

public class AcceptInviteCommandHandler: ICommandHandler<AcceptInviteCommand>
{
    private readonly IOrganizationRoleRepository _organizationRoleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationInviteRepository _organizationInviteRepository;
    private readonly IOrganizationRepository _organizationRepository;

    public AcceptInviteCommandHandler(
        IOrganizationRoleRepository organizationRoleRepository,
        IUserRepository userRepository, 
        IOrganizationInviteRepository organizationInviteRepository, 
        IOrganizationRepository organizationRepository)
    {
        _organizationRoleRepository = organizationRoleRepository;
        _userRepository = userRepository;
        _organizationInviteRepository = organizationInviteRepository;
        _organizationRepository = organizationRepository;
    }
    
    
    public async Task Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        var invite = await _organizationInviteRepository.FindById(request.InviteId);
        OrganizationInvite.ThrowIfDoesntExist(invite);
        invite.ThrowIfExpired();
        invite.ThrowIfNotAnOwner(request.Authorized);

        var organization = await _organizationRepository.FindById(invite.OrganizationId);
        Organization.ThrowIfDoesntExist(organization);
        
        var user = await _userRepository.FindUserById(invite.UserId);
        User.ThrowIfDoesntExist(user);

        var role = await _organizationRoleRepository.FindByName("MEMBER");
        var member = OrganizationMember.Create(user, organization, role);
        
        organization.AddMember(member);
        _organizationRepository.Update(organization);
        _organizationInviteRepository.Delete(invite);
    }
}