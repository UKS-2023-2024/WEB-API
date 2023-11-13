using Application.Shared;
using Domain.Auth;

namespace Application.Repositories.Commands.HandleRepositoryMembers.AddRepositoryMember;

public sealed record AddRepositoryMemberCommand(string Token) : ICommand;