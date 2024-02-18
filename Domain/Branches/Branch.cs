using Domain.Auth;
using Domain.Repositories;
using Domain.Branches.Exceptions;
using Domain.Tasks;

namespace Domain.Branches
{
    public class Branch
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } 
        public string OriginalName { get; private set; } 
        public Guid RepositoryId { get; private set; }
        public Repository Repository { get; private set; }
        public bool IsDefault { get; private set; }
        public Guid OwnerId { get; private set; }
        public User Owner { get; private set; }
        public bool Deleted { get; private set; }
        public string? CreatedFrom { get; set; }
        public List<PullRequest> FromPullRequests { get; private set; } = new();
        public List<PullRequest> ToPullRequests { get; private set; } = new();

        private Branch(string name, Guid repositoryId, bool isDefault, Guid ownerId)
        {
            Name = name;
            RepositoryId = repositoryId;
            IsDefault = isDefault;
            OwnerId = ownerId;
            Deleted = false;
            OriginalName = name;
        }

        public static Branch Create(string name, Guid repositoryId, bool isDefault, Guid ownerId)
        {
            return new Branch(name, repositoryId, isDefault, ownerId);
        }


        public static Branch Create(string name, Guid repositoryId, bool isDefault, Guid ownerId, string createdFrom)
        {
            var branch = new Branch(name, repositoryId, isDefault, ownerId)
            {
                CreatedFrom = createdFrom
            };
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
