using Application.Shared;
using Domain.Tasks;

namespace Application.Labels.Commands.Update;

public record UpdateLabelCommand(Guid Id, string Title, string Description, string Color): ICommand<Label>;