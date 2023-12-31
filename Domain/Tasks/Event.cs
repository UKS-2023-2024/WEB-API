﻿using Domain.Auth;
using Domain.Tasks.Enums;

namespace Domain.Tasks;
public class Event
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid CreatorId { get; private set; }
    public User? Creator { get; private set; }
    public EventType EventType { get; private set; }
    public Guid TaskId { get; private set; }
    public Task? Task { get; private set; }

    public Event()
    {
    }
    public Event(string title,EventType type, Guid creatorId)
    {
        Title = title;
        CreatedAt = DateTime.UtcNow;
        CreatorId = creatorId;
        EventType = type;
    }
    public Event(string title,EventType type, Guid creatorId, Guid taskId)
    {
        Title = title;
        CreatedAt = new DateTime().ToUniversalTime();
        CreatorId = creatorId;
        TaskId = taskId;
        EventType = type;
    }
}