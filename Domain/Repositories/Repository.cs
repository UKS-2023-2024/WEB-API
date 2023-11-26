using Domain.Auth;
using Domain.Branches;
using Domain.Milestones;
using Domain.Organizations;
using Domain.Repositories.Exceptions;

namespace Domain.Repositories
{
    public class Repository
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsPrivate { get; private set; }
        public Organization? Organization { get; private set; }
        private List<RepositoryMember> _members = new();
        private List<RepositoryInvite> _pendingInvites = new();
        public IReadOnlyList<RepositoryMember> Members => new List<RepositoryMember>(_members);
        public IReadOnlyList<RepositoryInvite> PendingInvites => new List<RepositoryInvite>(_pendingInvites);
        public List<User> StarredBy { get; private set; }
        public List<Milestone> Milestones { get; private set; }
        public List<Branch> Branches { get; private set; } = new();


        private Repository() { }

        private Repository(string name, string description, bool isPrivate, Organization? organization, List<User> starredBy)
        {
            Name = name;
            Description = description;
            IsPrivate = isPrivate;  
            Organization = organization;
            StarredBy = starredBy;
        }

        public static Repository Create(string name, string description, bool isPrivate, Organization? organization)
        {
            return new Repository(name, description, isPrivate, organization, new());
        }

        public static Repository Create(Guid id, string name, string description, bool isPrivate, Organization? organization)
        {
            var repository = new Repository(name, description, isPrivate, organization, new())
             {
                 Id = id
             };
            return repository;
        }

        public void AddMember(RepositoryMember member)
        {
            _members.Add(member);
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

        public void ThrowIfAlreadyStarredBy(Guid userId)
        {
            if(StarredBy.Any(user=> user.Id == userId))
                throw new RepositoryAlreadyStarredException();
        }
        public void ThrowIfNotStarredBy(Guid userId)
        {
            if(StarredBy.All(u => u.Id != userId))
                throw new RepositoryNotStarredException();
        }
        public static void ThrowIfDoesntExist(Repository? user)
        {
            if (user is null) throw new RepositoryNotFoundException();
        }
        public void ThrowIfUserNotMemberAndRepositoryPrivate(RepositoryMember? repositoryMember)
        {
            if(IsPrivate && repositoryMember == null)
                throw new RepositoryInaccessibleException();
        }

        public void AddBranch(Branch branch)
        {
            Branches.Add(branch);
        }
    }
}
