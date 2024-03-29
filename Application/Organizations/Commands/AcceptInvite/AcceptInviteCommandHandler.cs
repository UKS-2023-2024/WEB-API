using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Organizations.Commands.AcceptInvite;

public class AcceptInviteCommandHandler: ICommandHandler<AcceptInviteCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IOrganizationInviteRepository _organizationInviteRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IGitService _gitService;

    public AcceptInviteCommandHandler(
        IUserRepository userRepository, 
        IOrganizationInviteRepository organizationInviteRepository, 
        IOrganizationRepository organizationRepository,
        IGitService gitService)
    {
        _userRepository = userRepository;
        _organizationInviteRepository = organizationInviteRepository;
        _organizationRepository = organizationRepository;
        _gitService = gitService;
    }
    
    
    public async Task Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        var invite = await _organizationInviteRepository.FindById(request.InviteId);
        OrganizationInvite.ThrowIfDoesntExist(invite);
        invite!.ThrowIfExpired();

        var organization = await _organizationRepository.FindById(invite.OrganizationId);
        Organization.ThrowIfDoesntExist(organization);
        
        var user = await _userRepository.FindUserById(invite.UserId);
        User.ThrowIfDoesntExist(user);

        organization!.AddMember(user!);
        _organizationRepository.Update(organization);
        _organizationInviteRepository.Delete(invite);
        await _gitService.AddOrganizationMember(user!, organization);
    }
}