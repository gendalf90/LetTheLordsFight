using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UsersDomain.Exceptions.Registration;
using UsersDomain.Repositories.Registration;
using UsersService.Database;

namespace UsersService.Domain
{
    class RequestsRepository : IRequests
    {
        private readonly Context dbContext;

        public RequestsRepository(Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<RequestDto> GetByIdAsync(Guid id)
        {
            var entry = await dbContext.RegistrationRequests.SingleOrDefaultAsync(request => request.Id == id);

            if(entry == null)
            {
                throw new RequestException($"Registration request with id {id} not found");
            }

            return MapFromDatabaseDto(entry);
        }

        private RequestDto MapFromDatabaseDto(RegistrationRequestDto dto)
        {
            return new RequestDto
            {
                Id = dto.Id,
                Login = dto.Login,
                Password = dto.Password,
                TTL = TimeSpan.FromSeconds(dto.TTLSeconds)
            };
        }

        public async Task SaveAsync(RequestDto data)
        {
            var databaseData = MapToDatabaseDto(data);
            await TrySaveAsync(databaseData);
        }

        private RegistrationRequestDto MapToDatabaseDto(RequestDto dto)
        {
            return new RegistrationRequestDto
            {
                Id = dto.Id,
                Login = dto.Login,
                Password = dto.Password,
                TTLSeconds = dto.TTL.TotalSeconds
            };
        }

        private async Task TrySaveAsync(RegistrationRequestDto dto)
        {
            try
            {
                dbContext.RegistrationRequests.Add(dto);
                await dbContext.SaveChangesAsync();
            }
            catch(DbUpdateException e)
            {
                e.ThrowIfDublicateEntry(new RequestException($"Attempt to save dublicate of registration request with id {dto.Id}"));
                throw;
            }
        }
    }
}
