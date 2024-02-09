using Application.Shared;

namespace Application.Repositories.Queries.FindNumberOfForks;

public record FindNumberOfForksCommand(Guid UserId,Guid RepositoryId) :ICommand<int>;