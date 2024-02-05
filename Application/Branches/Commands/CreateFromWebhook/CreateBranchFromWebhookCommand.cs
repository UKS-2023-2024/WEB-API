using Application.Shared;

namespace Application.Branches.Commands.CreateFromWebhook;

public record CreateBranchFromWebhookCommand(string? Username, string? RepositoryName, string? RefName): ICommand;
