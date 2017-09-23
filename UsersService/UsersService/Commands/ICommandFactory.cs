﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersService.Common;

namespace UsersService.Commands
{
    interface ICommandFactory
    {
        ICommand GetCreateUserCommand(CreateUserData data);
    }
}
