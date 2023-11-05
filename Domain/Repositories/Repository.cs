using Domain.Auth;
using Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class Repository
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsPrivate { get; private set; }
        public Organization? Organization { get; private set; }
        public List<RepositoryMember> Members { get; private set; }
        public List<User> PendingMembers { get; private set; }

        private Repository() { }

        private Repository(string name, string description, bool isPrivate, Organization? organization, List<RepositoryMember> members, List<User> pendingMembers)
        {
            Name = name;
            Description = description;
            IsPrivate = isPrivate;  
            Organization = organization;
            Members = members;
            PendingMembers = pendingMembers;
        }

        public static Repository Create(string name, string description, bool isPrivate, Organization? organization)
        {
            return new Repository(name, description, isPrivate, organization, new(), new());
        }

        public void AddMember(RepositoryMember member)
        {
            Members.Add(member);
        }

    }
}
