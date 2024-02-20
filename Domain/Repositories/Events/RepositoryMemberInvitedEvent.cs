using MediatR;

namespace Domain.Repositories.Events;

public record RepositoryMemberInvitedEvent(Guid InviteId): INotification;