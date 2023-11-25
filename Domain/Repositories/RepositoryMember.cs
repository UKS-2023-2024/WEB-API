using Domain.Auth;
using Domain.Repositories.Exceptions;

namespace Domain.Repositories
{
    public class RepositoryMember
    {
        public Guid Id { get; private set; }
        public User Member { get; private set; }
        public Repository Repository { get; private set; }
        public Guid RepositoryId { get; private set; }
        public RepositoryMemberRole Role { get; private set; }
        public bool Deleted { get; private set; }
        public RepositoryMember()
        {
        }
        public RepositoryMember(User member, Repository repository, RepositoryMemberRole role)
        {
            Member = member;
            Repository = repository;
            Role = role;
            Deleted = false;
        }

        public static RepositoryMember Create(User member, Repository repository, RepositoryMemberRole role)
        {
            return new RepositoryMember(member, repository, role);
        }
        
        public static void ThrowIfDoesntExist(RepositoryMember? member)
        {
            if (member is null) throw new RepositoryMemberNotFoundException();
        }
        public void ThrowIfNotAdminPrivileges()
        {
            if (Role is not (RepositoryMemberRole.OWNER or RepositoryMemberRole.ADMIN)) throw new MemberNotOwnerException();
        }

        public void ActivateMemberAgain()
        {
            Deleted = false;
        }

        public void Delete()
        {
            Deleted = true;
        }
    }
}
