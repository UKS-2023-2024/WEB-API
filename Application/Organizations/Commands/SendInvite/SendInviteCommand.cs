using Application.Shared;

namespace Application.Organizations.Commands.SendInvite;

public record SendInviteCommand(Guid Authorized, Guid OrganizationId, Guid MemberId) : ICommand;
    
    
