using Application.Repositories.Commands.Create.CreateForOrganization;
using Application.Shared;

namespace Application.Repositories.Commands.HandleRepositoryMembers.AddRepositoryMember;

public class AddRepositoryMemberCommandHandler : ICommandHandler<AddRepositoryMemberCommand>
{
    public Task Handle(AddRepositoryMemberCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}