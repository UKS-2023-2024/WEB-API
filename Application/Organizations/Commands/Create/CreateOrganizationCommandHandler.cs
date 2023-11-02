﻿using Application.Organizations.Commands.Create;
using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Organizations;
using Domain.Organizations.Interfaces;

namespace Application.Organizations.Commands.Create;

public class CreateOrganizationCommandHandler: ICommandHandler<CreateOrganizationCommand, Guid>
{
    private IUserRepository _userRepository;
    private IOrganizationRepository _organizationRepository;
    private IOrganizationMemberRepository _organizationMemberRepository;

    public CreateOrganizationCommandHandler(IUserRepository userRepository, IOrganizationRepository organizationRepository,
        IOrganizationMemberRepository organizationMemberRepository)
    {
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
        _organizationMemberRepository = organizationMemberRepository;
    }

    public async Task<Guid> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        User creator = await _userRepository.FindUserById(request.creatorId);

        List<User> pendingMembers = new List<User>();
        foreach (Guid id in request.PendingMembers)
        {
            pendingMembers.Add(await _userRepository.FindUserById(id));
        }
        Organization newOrganization = Organization.Create(request.Name, request.ContactEmail, pendingMembers);
        OrganizationMember newMember = OrganizationMember.Create(creator, newOrganization);
        newOrganization.Members.Add(newMember);

        _organizationRepository.Create(newOrganization);
        return newOrganization.Id;
    }
}