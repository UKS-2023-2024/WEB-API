using Application.Shared;

namespace Application.Labels.Create;

public record CreateLabelCommand(Guid CreatorId, Guid RepositoryId, string Title, string Color, bool isDefaultLabel, string? Description): ICommand<Guid>;
