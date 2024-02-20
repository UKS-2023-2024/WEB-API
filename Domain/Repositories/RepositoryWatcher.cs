using Domain.Auth;
using Domain.Repositories.Enums;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class RepositoryWatcher
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public Guid RepositoryId { get; private set; }
        public Repository Repository { get; private set; }
        public WatchingPreferences WatchingPreferences { get; private set; }
        private RepositoryWatcher() { }

        private RepositoryWatcher(Guid userId, WatchingPreferences watchingPreferences, Guid repositoryId)
        {
            UserId = userId;
            WatchingPreferences = watchingPreferences;
            RepositoryId = repositoryId;
        }

        public static RepositoryWatcher Create(Guid userId, WatchingPreferences watchingPreferences, Guid repositoryId)
        {
            return new RepositoryWatcher(userId, watchingPreferences, repositoryId);
        }

        public void ChangeWatchingPreferences(WatchingPreferences watchingPreferences)
        {
            WatchingPreferences = watchingPreferences;
        }
    }
}
