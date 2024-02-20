using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Commands.WatchingRepository.UnwatchRepository;


public sealed record UnwatchRepositoryCommand(User User, Guid RepositoryId) : ICommand;