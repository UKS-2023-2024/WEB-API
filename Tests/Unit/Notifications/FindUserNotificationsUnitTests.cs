﻿using Shouldly;
using Moq;
using Domain.Notifications.Interfaces;
using Application.Notifications.Queries.FindUserNotifications;
using Domain.Notifications;
using Domain.Auth;
using Domain.Shared;

namespace Tests.Unit.Notifications
{
    public class FindUserNotificationsUnitTests
    {
        private Mock<INotificationRepository> _notificationRepositoryMock;

        public FindUserNotificationsUnitTests()
        {
            _notificationRepositoryMock = new();
        }

        [Fact]
        async Task FindUserNotifications_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindUserNotificationsQuery(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), 5, 1);
            Notification notification = Notification.Create("", "", It.IsAny<User>(), DateTime.UtcNow);
            List<Notification> notifications = new() { notification };
            PagedResult<Notification> page = new PagedResult<Notification>(notifications, 1);
            _notificationRepositoryMock.Setup(x => x.FindByUserId(It.IsAny<Guid>(), 5, 1)).ReturnsAsync(page);

            var handler = new FindUserNotificationsQueryHandler(_notificationRepositoryMock.Object);

            //Act
            var retList = await handler.Handle(query, default);

            //Assert
            retList.Data.ShouldNotBeEmpty();
            retList.TotalItems.ShouldBe(1);
        }
    }
}
