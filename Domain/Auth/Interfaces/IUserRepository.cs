﻿using Domain.Shared.Interfaces;

namespace Domain.Auth.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    public Task<User> FindUserByEmail(string email);
}