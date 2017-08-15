using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Commands
{
    interface ICommand
    {
        Task ExecuteAsync();
    }
}
