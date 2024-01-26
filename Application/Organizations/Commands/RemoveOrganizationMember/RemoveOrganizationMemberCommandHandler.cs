using Application.Shared;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
using Domain.Repositories.Interfaces;

namespace Application.Organizations.Commands.RemoveOrganizationMember;

public class RemoveOrganizationMemberCommandHandler: ICommandHandler<RemoveOrganizationMemberCommand>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    
    public RemoveOrganizationMemberCommandHandler(
        IOrganizationMemberRepository organizationMemberRepository,
        IOrganizationRepository organizationRepository,
        IRepositoryRepository repositoryRepository,
        IRepositoryMemberRepository repositoryMemberRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
        _organizationRepository = organizationRepository;
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }
    
    public async Task Handle(RemoveOrganizationMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.OrganizationMemberId,request.OrganizationId);
        OrganizationMember.ThrowIfDoesntExist(member);

        if (member.HasRole(OrganizationMemberRole.OWNER))
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