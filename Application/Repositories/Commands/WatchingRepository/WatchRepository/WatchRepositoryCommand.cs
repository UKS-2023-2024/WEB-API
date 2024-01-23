using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Commands.WatchingRepository.WatchRepository;


public sealed record WatchRepositoryCommand(User User, Guid RepositoryId) : ICommand;