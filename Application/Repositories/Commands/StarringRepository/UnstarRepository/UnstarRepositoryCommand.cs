using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Commands.StarringRepository.UnstarRepository;


public sealed record UnstarRepositoryCommand(User User, Guid RepositoryId) : ICommand;