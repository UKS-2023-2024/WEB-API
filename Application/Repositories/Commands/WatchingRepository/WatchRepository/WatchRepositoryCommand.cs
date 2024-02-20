using Application.Shared;
using Domain.Auth;
using Domain.Repositories.Enums;

namespace Application.Repositories.Commands.WatchingRepository.WatchRepository;


public sealed record WatchRepositoryCommand(User User, Guid RepositoryId, WatchingPreferences preferences) : ICommand;