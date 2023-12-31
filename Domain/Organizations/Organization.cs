﻿using Domain.Auth;
using Domain.Organizations.Exceptions;
using Domain.Repositories;

namespace Domain.Organizations;

public class Organization
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string ContactEmail { get; private set; }
    private List<OrganizationMember> _members = new();
    public IReadOnlyList<OrganizationMember> Members => new List<OrganizationMember>(_members);
    public string? Description { get; private set; }
    public string? Url { get; private set; }
    public string? Location { get; private set; }
    public List<OrganizationInvite> PendingInvites { get; private set; }
    public List<Repository> Repositories { get; private set; }
    private Organization()
    {
    }

    private Organization(string name, string contactEmail, List<User> pendingMembers)
    {
        Name = name;
        ContactEmail = contactEmail;
        _members = new List<OrganizationMember>();
        PendingInvites = pendingMembers
            .Select(mem => OrganizationInvite.Create(mem.Id, Id))
            .ToList();
    }

    public void AddMember(OrganizationMember member)
    {
        _members.Add(member);
    }
    
    public static Organization Create(string name, string contactEmail, List<User> pendingMembers)
    {
        Organization newOrganization = new Organization(name, contactEmail, pendingMembers);
        return newOrganization;
    }

    public static Organization Create(Guid id, string name, string contactEmail, List<User> pendingMembers)
    {
        Organization newOrganization = new Organization(name, contactEmail, pendingMembers);
        newOrganization.Id = id;
        return newOrganization;
    }

    public static void ThrowIfDoesntExist(Organization? organization)
    {
        if (organization is null) throw new OrganizationNotFoundException();
    }
}