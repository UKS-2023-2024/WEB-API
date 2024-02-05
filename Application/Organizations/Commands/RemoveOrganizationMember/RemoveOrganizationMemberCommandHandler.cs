using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Organizations.Commands.RemoveOrganizationMember;

public class RemoveOrganizationMemberCommandHandler: ICommandHandler<RemoveOrganizationMemberCommand>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IGitService _gitService;
    private readonly IUserRepository _userRepository;
    
    public RemoveOrganizationMemberCommandHandler(
        IOrganizationMemberRepository organizationMemberRepository,
        IOrganizationRepository organizationRepository,
        IRepositoryRepository repositoryRepository,
        IRepositoryMemberRepository repositoryMemberRepository,
        IGitService gitService,
        IUserRepository userRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
        _organizationRepository = organizationRepository;
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _gitService = gitService;
        _userRepository = userRepository;
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

        var user = await _userRepository.FindUserById(request.OrganizationMemberId);
        User.ThrowIfDoesntExist(user);
        await _gitService.RemoveOrganizationMember(user!, organization);
    }
}