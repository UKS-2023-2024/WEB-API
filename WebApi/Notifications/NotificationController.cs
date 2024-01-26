using Application.Notifications.Commands.UpdateNotificationPreferences;
using Application.Notifications.Queries.FindUserNotifications;
using Domain.Auth.Enums;
using Domain.Notifications;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_API.Notifications.Dtos;
using WEB_API.Notifications.Presenters;
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

    [HttpGet("{pageSize}/{pageNumber}")]
    [Authorize]
    public async Task<IActionResult> FindAllLoggedUserNotifications(int pageSize, int pageNumber)
    {
        var id = _userIdentityService.FindUserIdentity(HttpContext.User);
        var ret = await _sender.Send(new FindUserNotificationsQuery(id, pageSize, pageNumber));
        PagedResult<NotificationPresenter> page = new(NotificationPresenter.MapNotificationsToNotificationPresenters(ret.Data), ret.TotalItems);
        return Ok(page);
    }
}