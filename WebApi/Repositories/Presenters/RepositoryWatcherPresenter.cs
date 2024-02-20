using Domain.Auth;
using Domain.Repositories;

namespace WEB_API.Repositories.Presenters
{
    public class RepositoryWatcherPresenter
    {
        public string Username { get; private set; }
        public string? Location { get; private set; }


        public RepositoryWatcherPresenter(RepositoryWatcher watcher)
        {
            Username = watcher.User.Username;
            Location = watcher.User.Location;
        }
        public static IEnumerable<RepositoryWatcherPresenter> MapRepositoryWatcherToRepositoryWatcherPresenters(
            IEnumerable<RepositoryWatcher> watchers)
        {
            return watchers.Select(w => new RepositoryWatcherPresenter(w));
        }
    }
}

