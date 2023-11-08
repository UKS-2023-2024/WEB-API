using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Commands.UnstarRepository;


public sealed record UnstarRepositoryCommand(User user, Guid repositoryId) : ICommand;