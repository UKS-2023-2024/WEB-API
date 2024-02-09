using Application.Shared;

namespace Application.Repositories.Commands.Fork;

public record ForkRepositoryCommand(Guid UserId, Guid RepositoryId) : ICommand<Guid>;