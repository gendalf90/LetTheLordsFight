using Cassandra;
using MapDomain.Repositories;
using MapDomain.ValueObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Linq;
using MapDomain.Factories;

namespace MapService.Queries
{
    public class SegmentQuery : IQuery
    {
        private readonly ISession session;

        private Func<Segment> getStrategy;
        private Segment segment;
        private JToken segmentData;
        private JToken objectsData;
        private SimpleStatement statement;
        private RowSet objectsRows;

        public SegmentQuery(ISession session, IMapFactory factory, int i, int j)
        {
            this.session = session;

            getStrategy = () =>
            {
                var map = factory.GetMap();
                return map[i, j];
            };
        }

        public SegmentQuery(ISession session, IMapFactory factory, float x, float y)
        {
            this.session = session;

            getStrategy = () =>
            {
                var map = factory.GetMap();
                return map[x, y];
            };
        }

        public async Task<string> GetJsonAsync()
        {
            LoadSegment();
            CreateSegmentData();
            await LoadObjectsDataAsync();
            return GetResult();
        }

        private void LoadSegment()
        {
            segment = getStrategy();
        }

        private void CreateSegmentData()
        {
            segmentData = new JObject
            {
                ["leftX"] = segment.LeftUpLocation.X,
                ["upY"] = segment.LeftUpLocation.Y,
                ["rightX"] = segment.RightDownLocation.X,
                ["downY"] = segment.RightDownLocation.Y,
                ["type"] = segment.Type.ToString()
            };
        }

        private async Task LoadObjectsDataAsync()
        {
            CreateStatement();
            await LoadObjectsRowsAsync();
            ConvertObjectsRowsToJson();
        }

        private void CreateStatement()
        {
            var query = @"select id, locationX, locationY 
                          from objects 
                          where visible = true 
                                and locationX > ? and locationX < ?
                                and locationY > ? and locationY < ?";

            var parameters = new[] 
            {
                segment.LeftUpLocation.X,
                segment.RightDownLocation.X,
                segment.RightDownLocation.Y,
                segment.LeftUpLocation.Y
            };

            statement = new SimpleStatement(query, parameters);
        }

        private async Task LoadObjectsRowsAsync()
        {
            objectsRows = await session.ExecuteAsync(statement);
        }

        private void ConvertObjectsRowsToJson()
        {
            JObject Convert(Row row)
            {
                var result = new
                {
                    Id = row.GetValue<string>("id"),
                    Position = new
                    {
                        X = row.GetValue<float>("locationX"),
                        Y = row.GetValue<float>("locationY")
                    }
                };
                
                return JObject.FromObject(result);
            }

            objectsData = new JArray(objectsRows.Select(Convert));
        }

        private string GetResult()
        {
            var result = new JObject();
            result.Add("segment", segmentData);
            result.Add("objects", objectsData);
            return result.ToString();
        }
    }
}
