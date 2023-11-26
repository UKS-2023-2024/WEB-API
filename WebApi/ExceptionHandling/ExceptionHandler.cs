using System.Net;
using Domain.Auth.Exceptions;
using Domain.Branches.Exceptions;
using Domain.Exceptions;
using Domain.Organizations.Exceptions;
using Domain.Repositories.Exceptions;
namespace WEB_API.ExceptionHandling;

public static class ExceptionHandler
{
    public static int GetStatusCode(this BaseException exception)
    {
        var code = exception switch
        {
            InvalidCredentialsException 
                or UnautorizedAccessException => HttpStatusCode.Unauthorized,
            UserNotFoundException 
                or RepositoryNotFoundException 
                or OrganizationNotFoundException
                or OrganizationInviteNotFoundException
                or OrganizationMemberNotFoundException
                or RepositoryMemberNotFoundException
                or RepositoryInviteNotFound
                or UserNotFoundException => HttpStatusCode.NotFound,
            UserWithThisEmailExistsException 
                or RepositoryWithThisNameExistsException
                or AlreadyOrganizationMemberException
                or BranchWithThisNameExistsException 
                or BranchIsAlreadyDefaultException => HttpStatusCode.Conflict,
            PermissionDeniedException 
                or MemberHasNoPrivilegeException
                or RepositoryInaccessibleException
                or NotInviteOwnerException
                or UserNotAOrganizationMemberException => HttpStatusCode.Forbidden,
            InvitationExpiredException
                or RepositoryAlreadyStarredException
                or RepositoryNotStarredException => HttpStatusCode.BadRequest
        };
        return (int)code;
    }
}