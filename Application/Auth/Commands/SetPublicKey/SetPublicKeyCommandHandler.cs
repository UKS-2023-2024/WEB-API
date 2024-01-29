using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Auth.Commands.SetPublicKey;

public class SetPublicKeyCommandHandler: ICommandHandler<SetPublicKeyCommand>
{

    private readonly IUserRepository _userRepository;
    private readonly IGitService _gitService;

    public SetPublicKeyCommandHandler(
        IUserRepository userRepository,
        IGitService gitService
    )
    {
        _userRepository = userRepository;
        _gitService = gitService;
    }

    public async Task Handle(SetPublicKeyCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserById(request.UserId);
        User.ThrowIfDoesntExist(user);
        await _gitService.SetPublicKey(user!, request.PublicKey);
    }
}