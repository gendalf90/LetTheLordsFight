﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}
