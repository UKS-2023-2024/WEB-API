﻿using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UnautorizedAccessException : BaseException
{
    public UnautorizedAccessException() : base("Unautorized access!")
    {
    }
}
