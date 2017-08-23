﻿using MapService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Commands
{
    interface ICommandFactory
    {
        ICommand GetAddMapObjectCommand(string id, MapObjectCreateData data);

        ICommand GetUpdateMapObjectCommand(string id, MapObjectUpdateData data);
    }
}
