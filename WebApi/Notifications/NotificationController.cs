using Application.Notifications.Commands.UpdateNotificationPreferences;
using Domain.Auth.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Notifications.Dtos;
using WEB_API.Shared.TokenHandler;
using WEB_API.Shared.UserIdentityService;


namespace WEB_API.Notifications;

[ApiController]
[Route("notifications")]
public class NotificationController: ControllerBase
{
    private readonly ISender _sender;
    private readonly ITokenHandler _tokenHandler;
    private readonly IUserIdentityService _userIdentityService;

    public NotificationController(ISender mediator, ITokenHandler tokenHandler, IUserIdentityService userIdentityService)
    {
        _sender = mediator;
        _tokenHandler = tokenHandler;
        _userIdentityService = userIdentityService;
    }

    [HttpPatch]
    [Authorize]
    [Route("preferences")]
    public async Task<IActionResult> Update([FromBody] UpdateNotificationPreferencesDto data)
    {
        var id = _userIdentityService.FindUserIdentity(HttpContext.User);
        NotificationPreferences pref = NotificationPreferences.DONTNOTIFY;
        if (data.Email && data.Github)
            pref = NotificationPreferences.BOTH;
        else if (data.Github)
            pref = NotificationPreferences.GITHUB;
        else if (data.Email)
            pref = NotificationPreferences.EMAIL;
        await _sender.Send(new UpdateNotificationPreferencesCommand(id, pref));
        return Ok();
    }
}