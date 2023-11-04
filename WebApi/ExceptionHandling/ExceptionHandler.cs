using System.Net;
using Domain.Exceptions;

namespace WEB_API.ExceptionHandling;

public static class ExceptionHandler
{
    public static int GetStatusCode(this BaseException exception)
    {
        HttpStatusCode code = exception switch
        {
            InvalidCredentialsException => HttpStatusCode.Unauthorized,
            UserNotFoundException => HttpStatusCode.NotFound,
            UserWithThisEmailExistsException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };

        return (int)code;
    }
}