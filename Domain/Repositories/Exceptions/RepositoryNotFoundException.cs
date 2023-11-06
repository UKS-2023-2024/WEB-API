
﻿using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;


public class RepositoryNotFoundException : BaseException
{
    public RepositoryNotFoundException() : base("Repository not found!")
    {
    }
}