using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorageService.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
    }
}
