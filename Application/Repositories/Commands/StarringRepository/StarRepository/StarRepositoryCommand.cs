using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Commands.StarringRepository.StarRepository;


public sealed record StarRepositoryCommand(User User, Guid RepositoryId) : ICommand;