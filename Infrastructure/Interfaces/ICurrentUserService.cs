﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Interfaces
{
    public interface ICurrentUserService
    {
        string UserName { get; }
    }
}