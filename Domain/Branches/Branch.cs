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
        private Branch(string name, Guid repositoryId, bool isDefault)
        {
            Name = name;
            RepositoryId = repositoryId;
            IsDefault = isDefault;
        }

        public static Branch Create(string name, Guid repositoryId, bool isDefault)
        {
            return new Branch(name, repositoryId, isDefault);
        }
    }
}
