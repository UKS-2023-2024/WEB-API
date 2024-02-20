using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
using Domain.Shared.Interfaces;

namespace Application.Organizations.Commands.Delete;

public class DeleteOrganizationCommandHandler: ICommandHandler<DeleteOrganizationCommand>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IGitService _gitService;
    private readonly IUserRepository _userRepository;
    public DeleteOrganizationCommandHandler(IOrganizationMemberRepository organizationMemberRepository, IOrganizationRepository organizationRepository, IGitService gitService, IUserRepository userRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
        _organizationRepository = organizationRepository;
        _gitService = gitService;
        _userRepository = userRepository;
    }

    public async Task Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
    {
        var sender =
            await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.UserId, request.OrganizationId);
        sender.ThrowIfNotOwner();
        
        var organization = _organizationRepository.Find(request.OrganizationId);
        _organizationRepository.Delete(organization!);

        var user = await _userRepository.FindUserById(request.UserId);
        User.ThrowIfDoesntExist(user);
        await _gitService.DeleteOrganization(user!, organization!);
    }
}