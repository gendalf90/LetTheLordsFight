using MapDomain.Factories;
using MapDomain.Repositories;
using MapDomain.Services;
using MapService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Commands
{
    class CommandFactory : ICommandFactory
    {
        private readonly IUserValidationService userValidationService;
        private readonly IMapObjectsRepository mapObjectsRepository;
        private readonly IMapFactory mapFactory;

        public CommandFactory(IUserValidationService userValidationService,
                              IMapObjectsRepository mapObjectsRepository,
                              IMapFactory mapFactory)
        {
            this.userValidationService = userValidationService;
            this.mapObjectsRepository = mapObjectsRepository;
            this.mapFactory = mapFactory;
        }

        public ICommand GetAddMapObjectCommand(string id, MapObjectCreateData data)
        {
            return new AddObjectCommand(userValidationService, mapObjectsRepository, mapFactory, data, id);
        }

        public ICommand GetUpdateMapObjectCommand(string id, MapObjectUpdateData data)
        {
            return new UpdateObjectCommand(userValidationService, mapObjectsRepository, data, id);
        }
    }
}
