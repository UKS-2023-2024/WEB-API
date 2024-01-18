using Application.Shared;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
using Domain.Repositories.Interfaces;

namespace Application.Organizations.Commands.RemoveOrganizationMember;

public class RemoveOrganizationMemberCommandHandler: ICommandHandler<RemoveOrganizationMemberCommand>
{
    private readonly IPermissionService _permissionService;
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IOrganizationRoleRepository _organizationRoleRepository;
    
    public RemoveOrganizationMemberCommandHandler(
        IOrganizationMemberRepository organizationMemberRepository,
        IPermissionService permissionService,
        IOrganizationRepository organizationRepository,
        IRepositoryRepository repositoryRepository,
        IRepositoryMemberRepository repositoryMemberRepository,
        IOrganizationRoleRepository organizationRoleRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
        _permissionService = permissionService;
        _organizationRepository = organizationRepository;
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _organizationRoleRepository = organizationRoleRepository;
    }
    
    public async Task Handle(RemoveOrganizationMemberCommand request, CancellationToken cancellationToken)
    {
        await _permissionService.ThrowIfNoPermission(new PermissionParams
        {
            Authorized = request.OwnerId,
            OrganizationId = request.OrganizationId,
            Permission = "admin"
        });

        var ownerRole = await _organizationRoleRepository.FindByName("OWNER");
        
        var member = await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.OrganizationMemberId,request.OrganizationId);
        if (member is null || member.Deleted) 
            throw new OrganizationMemberNotFoundException();

        if (member.HasRole(ownerRole))
            throw new CantRemoveOrganizationOwnerException();

        var organization =  await _organizationRepository.FindById(request.OrganizationId);

        organization!.RemoveMember(member);
        _organizationRepository.Update(organization);

        var repositoriesToRemoveUserFrom = await
            _repositoryRepository.FindOrganizationRepositoriesThatContainsUser(member.MemberId, organization.Id);

        foreach (var repository in repositoriesToRemoveUserFrom)
        {
            var repositoryMember = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(member.MemberId, repository.Id);
            repository.RemoveMember(repositoryMember);
            _repositoryRepository.Update(repository);
        }
    }
}