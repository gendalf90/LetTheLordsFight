using ArmiesService.Controllers.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArmiesService.Commands.CreateArmy
{
    class Command : ICommand
    {
        public Command(ArmyDto data)
        {

        }

        public Task ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
