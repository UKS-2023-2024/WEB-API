using Application.Shared;
using Domain.Auth;

namespace Application.Auth.Commands.Update;
public sealed record UpdateUserCommand(Guid id, string fullName, string bio, string company, string location, string website, List<SocialAccount> socialAccounts) : ICommand<User>;
