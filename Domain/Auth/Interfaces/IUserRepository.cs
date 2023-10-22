﻿namespace Library.Auth.Interfaces;

public interface IUserRepository
{
    List<User> FindAll();
    void Create(User user);
}