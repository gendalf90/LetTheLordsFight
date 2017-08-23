using Cassandra;
using MapDomain.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Queries
{
    public class ObjectQuery : IQuery
    {
        private readonly ISession session;
        private readonly IUserValidationService validation;
        private readonly string id;

        private SimpleStatement statement;
        private Row row;

        public ObjectQuery(ISession session, IUserValidationService validation, string id)
        {
            this.session = session;
            this.validation = validation;
            this.id = id;
        }

        public async Task<string> GetJsonAsync()
        {
            Validate();
            CreateStatement();
            await LoadObjectRowAsync();
            return GetResult();
        }

        private void Validate()
        {
            validation.CurrentCanViewThisMapObject(id);
        }

        private void CreateStatement()
        {
            var query = @"select id, locationX, locationY, visible
                          from objects
                          where id = ?";
            statement = new SimpleStatement(query, id);
        }

        private async Task LoadObjectRowAsync()
        {
            var rows = await session.ExecuteAsync(statement);
            row = rows.First();
        }

        private string GetResult()
        {
            var result = new
            {
                Id = row.GetValue<string>("id"),
                Position = new
                {
                    X = row.GetValue<float>("locationX"),
                    Y = row.GetValue<float>("locationY")
                },
                Visible = row.GetValue<bool>("visible")
            };
            return JsonConvert.SerializeObject(result);
        }
    }
}
