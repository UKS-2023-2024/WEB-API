using System.Net;
using Domain.Auth.Exceptions;
using Domain.Exceptions;
using Domain.Organizations.Exceptions;
using Domain.Repositories.Exceptions;
namespace WEB_API.ExceptionHandling;

public static class ExceptionHandler
{
    public static int GetStatusCode(this BaseException exception)
    {
        HttpStatusCode code = exception switch
        {
            InvalidCredentialsException 
                or UnautorizedAccessException => HttpStatusCode.Unauthorized,
            UserNotFoundException 
                or RepositoryNotFoundException 
                or OrganizationNotFoundException
                or OrganizationInviteNotFoundException
                or OrganizationMemberNotFoundException
                or UserNotFoundException => HttpStatusCode.NotFound,
            UserWithThisEmailExistsException 
                or RepositoryWithThisNameExistsException
                or AlreadyOrganizationMemberException => HttpStatusCode.Conflict,
            PermissionDeniedException 
                or NotInviteOwnerException => HttpStatusCode.Forbidden,
            InvitationExpiredException => HttpStatusCode.BadRequest
        };
        return (int)code;
    }
}