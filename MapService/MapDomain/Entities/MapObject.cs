using MapDomain.Common;
using MapDomain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MapDomain.Entities
{
    public class MapObject
    {
        private readonly string id;
        private readonly Map map;
        private Location location;
        private Location destination;
        private bool isVisible;

        public MapObject(MapObjectRepositoryData data, Map map)
        {
            this.map = map;
            this.id = data.Id;
            this.location = new Location(data.LocationX, data.LocationY);
            this.destination = new Location(data.DestinationX, data.DestinationY);
            this.isVisible = data.IsVisible;
        }

        public void SetPosition(Location location)
        {
            ValidateLocation(location);
            SetLocation(location);
            SetDestination(location);
            SetVisible();
        }

        private void SetLocation(Location location)
        {
            this.location = location;
        }

        public void GoTo(Location destination)
        {
            ValidateLocation(destination);
            SetDestination(destination);
        }

        private void SetDestination(Location destination)
        {
            this.destination = destination;
        }

        private void ValidateLocation(Location location)
        {
            if(location.X < 0 || location.X >= map.Width || location.Y < 0 || location.Y >= map.Height)
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private void SetVisible()
        {
            var currentSegment = map[location.X, location.Y];
            isVisible = currentSegment.Type != SegmentType.Forest;
        }

        //public void SetSpeedCoefficient(float coefficient)
        //{

        //}

        public void UpdateMoving(TimeSpan elapsedTime)
        {
            var isStay = location == destination;

            if (isStay)
            {
                return;
            }

            var speed = GetSpeed();
            var traveledDistance = speed * (float)elapsedTime.TotalSeconds;
            var totalDistance = GetTotalDistance();
            var traveledPart = totalDistance / traveledDistance;
            location = GetNextLocation(traveledPart);
        }

        private float GetSpeed()
        {
            var segment = map[location.X, location.Y];
            return segment.Speed;
        }

        private float GetTotalDistance()
        {
            var fromVector = new Vector2(location.X, location.Y);
            var toVector = new Vector2(destination.X, destination.Y);
            return Vector2.Distance(fromVector, toVector);
        }

        private Location GetNextLocation(float traveledPart)
        {
            var isFinish = traveledPart >= 1;

            if (isFinish)
            {
                return destination;
            }

            var fromVector = new Vector2(location.X, location.Y);
            var toVector = new Vector2(destination.X, destination.Y);
            var movingVector = toVector - fromVector;
            var traveledVector = movingVector * traveledPart;
            var resultVector = fromVector + traveledVector;
            return new Location(resultVector.X, resultVector.Y);
        }

        public MapObjectRepositoryData GetRepositoryData()
        {
            return new MapObjectRepositoryData
            {
                Id = id,
                LocationX = location.X,
                LocationY = location.Y,
                DestinationX = destination.X,
                DestinationY = destination.Y,
                IsVisible = isVisible
            };
        }
    }
}
