using Domain.Repositories.Enums;

namespace WEB_API.Repositories.Dtos
{
    public class WatchRepositoryDto
    {
        public Guid RepositoryId { get; set; }
        public WatchingPreferences WatchingPreferences { get; set; }

    }
}
