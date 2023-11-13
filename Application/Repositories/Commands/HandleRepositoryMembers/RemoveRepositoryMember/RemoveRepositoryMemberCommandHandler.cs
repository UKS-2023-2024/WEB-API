using Application.Shared;

namespace Application.Repositories.Commands.HandleRepositoryMembers.RemoveRepositoryMember;

public class RemoveRepositoryMemberCommandHandler : ICommandHandler<RemoveRepositoryMemberCommand>
{
    public Task Handle(RemoveRepositoryMemberCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}