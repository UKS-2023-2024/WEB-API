using Domain.Auth;
using Domain.Repositories;
using System.Data.Entity.Migrations.Model;
using System.Security.Policy;
using Domain.Branches.Exceptions;

namespace Domain.Branches
{
    public class Branch
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } 
        public Guid RepositoryId { get; private set; }
        public Repository Repository { get; private set; }
        public bool IsDefault { get; private set; }
        public Guid OwnerId { get; private set; }
        public User Owner { get; private set; }
        public bool Deleted { get; private set; }
        private Branch(string name, Guid repositoryId, bool isDefault, Guid ownerId)
        {
            Name = name;
            RepositoryId = repositoryId;
            IsDefault = isDefault;
            OwnerId = ownerId;
            Deleted = false;
        }

        public static Branch Create(string name, Guid repositoryId, bool isDefault, Guid ownerId)
        {
            return new Branch(name, repositoryId, isDefault, ownerId);
        }


        public static Branch Create(Guid id, string name, Guid repositoryId, bool isDefault, Guid ownerId)
        {
            Branch branch = new Branch(name, repositoryId, isDefault, ownerId);
            branch.Id = id;
            return branch;
        }

        public static Branch Create(Guid id, string name, Guid repositoryId, bool isDefault, Guid ownerId, bool deleted)
        {
            Branch branch = new Branch(name, repositoryId, isDefault, ownerId);
            branch.Id = id;
            branch.Deleted = deleted;
            return branch;
        }

        public static void ThrowIfDoesntExist(Branch? branch)
        {
            if (branch is not null) return;
            throw new BranchNotFoundException();
        }


        public void Update(string name)
        {
            Name = name;
        }

        public void ChangeDefault(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public void Delete()
        {
            Deleted = true;
        }

        public void Restore()
        {
            Deleted = false;
        }

     }
}
