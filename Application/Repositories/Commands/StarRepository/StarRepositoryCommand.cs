using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Commands.StarRepository;


public sealed record StarRepositoryCommand(User user, Guid repositoryId) : ICommand;