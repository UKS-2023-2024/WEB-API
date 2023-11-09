using Domain.Auth;
using Domain.Organizations;

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
        public List<User> StarredBy { get; private set; }

        private Repository() { }

        private Repository(string name, string description, bool isPrivate, Organization? organization, List<RepositoryMember> members, List<User> pendingMembers, List<User> starredBy)
        {
            Name = name;
            Description = description;
            IsPrivate = isPrivate;  
            Organization = organization;
            Members = members;
            PendingMembers = pendingMembers;
            StarredBy = starredBy;
        }

        public static Repository Create(string name, string description, bool isPrivate, Organization? organization)
        {
            return new Repository(name, description, isPrivate, organization, new(), new(), new());
        }

        public static Repository Create(Guid id, string name, string description, bool isPrivate, Organization? organization)
        {
            var repository = new Repository(name, description, isPrivate, organization, new(), new(), new())
             {
                 Id = id
             };
            return repository;
        }

        public void AddMember(RepositoryMember member)
        {
            Members.Add(member);
        }
        
        public void AddToStarredBy(User user)
        {
            StarredBy.Add(user);
        }
        public void RemoveFromStarredBy(User user)
        {
            StarredBy.Remove(user);
        }

        public void Update(string name, string description, bool isPrivate)
        {
            Name = name;
            Description = description;
            
            if (!IsPrivate && isPrivate)
            {
                var starredUsersToRemove = new List<User>();
                starredUsersToRemove.AddRange(StarredBy.Where(starredUser => Members.All(m => m.Member != starredUser)));
                StarredBy = StarredBy.Except(starredUsersToRemove).ToList();
            }

            IsPrivate = isPrivate;
        }
    }
}
