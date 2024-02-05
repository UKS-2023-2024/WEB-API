using Domain.Auth;
using Domain.Branches;
using Domain.Milestones;
using Domain.Organizations;
using Domain.Repositories.Enums;
using Domain.Repositories.Exceptions;
using Domain.Tasks;

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
        public List<Tasks.Task> Tasks { get; private set; } = new();
        public List<Label> Labels { get; private set; } = new();
        public List<RepositoryWatcher> WatchedBy { get; private set; } = new();
        public string? HttpCloneUrl { get; private set; }
        public string? SshCloneUrl { get; private set; }

        private Repository() { }

        private Repository(string name, string description, bool isPrivate, Organization? organization, List<User> starredBy,User creator)
        {
            Name = name;
            Description = description;
            IsPrivate = isPrivate;  
            Organization = organization;
            StarredBy = starredBy;
            _members.Add(RepositoryMember.Create(creator, this, RepositoryMemberRole.OWNER));
        }

        public void SetCloneUrls(string? httpsCloneUrl, string? sshCloneUrl)
        {
            HttpCloneUrl = httpsCloneUrl;
            SshCloneUrl = sshCloneUrl;
        }

        public static Repository Create(string name, string description, bool isPrivate, Organization? organization,User creator)
        {
            return new Repository(name, description, isPrivate, organization, new(),creator);
        }

        public static Repository Create(Guid id, string name, string description, bool isPrivate, Organization? organization,User creator)
        {
            var repository = new Repository(name, description, isPrivate, organization, new(),creator)
             {
                 Id = id
             };
            return repository;
        }

        public RepositoryMember AddMember(User user)
        {
            var member = _members.FirstOrDefault(m => m.Member.Id == user.Id);
            if (member is not null)
            {
                member.ActivateMemberAgain();
                return member;
            }
            member = RepositoryMember.Create(user, this, RepositoryMemberRole.READ);
            _members.Add(member);
            return member;
        }
        
        public void RemoveMember(RepositoryMember repositoryMember)
        {
            var member = _members.FirstOrDefault(m => m.Id == repositoryMember.Id);
            if (member is null)
                throw new RepositoryMemberNotFoundException();
            member.Delete();
        }

        private void ThrowIfNoOrganizationRepositoryAccess(Guid userId)
        {
            var userIsOrganizationMember = Organization!.Members.Any(mem => mem.MemberId == userId);
            if (!userIsOrganizationMember) throw new RepositoryInaccessibleException();
        }

        public void ThrowIfUserCantAccessRepositoryData(Guid userId)
        {
            if (!IsPrivate) return;
            
            var userIsRepositoryMember = Members.Any(mem => mem.Member.Id == userId && !mem.Deleted);
            if (userIsRepositoryMember) return;
            
            var repositoryIsInOrganization = Organization != null;
            if (!repositoryIsInOrganization) throw new RepositoryInaccessibleException();
            ThrowIfNoOrganizationRepositoryAccess(userId);
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

        public void AddToWatchedBy(User user, WatchingPreferences preferences)
        {
            WatchedBy.Add(RepositoryWatcher.Create(user.Id, preferences, this.Id));
        }

        public void RemoveFromWatchedBy(RepositoryWatcher watcher)
        {
            WatchedBy.Remove(watcher);
        }

        public void ThrowIfNotWatchedBy(Guid userId)
        {
            if (WatchedBy.All(u => u.UserId != userId))
                throw new RepositoryNotWatchedException();
        }
    }
}
