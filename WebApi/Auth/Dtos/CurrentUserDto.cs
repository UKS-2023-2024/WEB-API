﻿using Domain.Auth;

namespace WEB_API.Auth.Dtos;

public class CurrentUserDto
{
    public string PrimaryEmail { get; }
    public string FullName { get; }
    public string Username { get; }
    public string? Bio { get; }
    public string? Location { get; }
    public string? Company { get; }
    
    public CurrentUserDto(User user)
    {
        PrimaryEmail = user.PrimaryEmail;
        FullName =  user.FullName;
        Username =  user.Username;
        Bio =  user.Bio;
        Location =  user.Location;
        Company =  user.Company;
    }
}