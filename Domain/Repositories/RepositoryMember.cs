using Domain.Auth;
using Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class RepositoryMember
    {
        public Guid Id { get; private set; }
        public User Member { get; private set; }
        public Repository Repository { get; private set; }
        public Guid RepositoryId { get; private set; }
        public RepositoryMemberRole Role { get; private set; }
        public RepositoryMember()
        {
        }
        public RepositoryMember(User member, Repository repository, RepositoryMemberRole role)
        {
            Member = member;
            Repository = repository;
            Role = role;
        }

        public static RepositoryMember Create(User member, Repository repository, RepositoryMemberRole role)
        {
            return new RepositoryMember(member, repository, role);
        }
    }
}
