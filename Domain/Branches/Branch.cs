using Domain.Auth;
using Domain.Repositories;


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
        private Branch(string name, Guid repositoryId, bool isDefault, Guid ownerId)
        {
            Name = name;
            RepositoryId = repositoryId;
            IsDefault = isDefault;
            OwnerId = ownerId;
        }

        public static Branch Create(string name, Guid repositoryId, bool isDefault, Guid ownerId)
        {
            return new Branch(name, repositoryId, isDefault, ownerId);
        }
    }
}
