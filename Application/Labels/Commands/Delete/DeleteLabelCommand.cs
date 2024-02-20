using Application.Shared;

namespace Application.Labels.Commands.Delete;

public record DeleteLabelCommand(Guid Id): ICommand<Guid>;