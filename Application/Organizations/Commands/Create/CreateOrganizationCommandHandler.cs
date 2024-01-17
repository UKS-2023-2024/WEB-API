using Application.Organizations.Commands.Create;
using Application.Shared;
using Domain.Auth;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;

namespace Application.Organizations.Commands.Create;

public class CreateOrganizationCommandHandler: ICommandHandler<CreateOrganizationCommand, Guid>
{
    private IUserRepository _userRepository;
    private IOrganizationRepository _organizationRepository;

    public CreateOrganizationCommandHandler(IUserRepository userRepository, IOrganizationRepository organizationRepository)
    {
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task<Guid> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var creator = await _userRepository.FindUserById(request.CreatorId);
        var role = await _organizationRepository.FindRole("OWNER");
        
        if (creator is null)
            throw new UserNotFoundException();
            
        List<User> pendingMembers = new();
        
        foreach (var id in request.PendingMembers)
        {
            var member = await _userRepository.FindUserById(id);
            if (member is null) continue;
            pendingMembers.Add(member);
        }

        var existingOrganization = await _organizationRepository.FindByName(request.Name);
        if (existingOrganization is not null)
            throw new OrganizationWithThisNameExistsException();

        var organization = Organization.Create(request.Name, request.ContactEmail, pendingMembers);

        
        organization.AddMember(creator,role!);
        await _organizationRepository.Create(organization);
        return organization.Id;
    }
}