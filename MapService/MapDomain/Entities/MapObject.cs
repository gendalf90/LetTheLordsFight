using MapDomain.Common;
using MapDomain.Exceptions;
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

        public MapObject(IMapObjectRepositoryData data, Map map)
        {
            this.map = map;
            this.id = data.Id;
            this.location = new Location(data.LocationX, data.LocationY);
            this.destination = new Location(data.DestinationX, data.DestinationY);
        }

        public MapObject(string id, Map map)
        {
            this.id = id;
            this.map = map;
        }

        public string Id
        {
            get => id;
        }

        public void SetPosition(Location location)
        {
            ValidateLocation(location);
            SetLocation(location);
            SetDestination(location);
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
                throw new BadLocationException();
            }
        }

        public bool IsMoving
        {
            get => location != destination;
        }

        public bool IsVisible
        {
            get
            {
                var currentSegment = map[location.X, location.Y];
                return currentSegment.Type != SegmentType.Forest;
            }
        }

        //public void SetSpeedCoefficient(float coefficient)
        //{

        //}

        public void UpdateMoving(TimeSpan elapsedTime)
        {
            if(!IsMoving)
            {
                return;
            }

            var elapsedSeconds = (float)elapsedTime.TotalSeconds;
            location = CalculateNextLocation(elapsedSeconds);
        }
        
        private Location CalculateNextLocation(float elapsedSeconds)
        {
            var speed = GetSpeed();
            var fromVector = new Vector2(location.X, location.Y);
            var toVector = new Vector2(destination.X, destination.Y);
            var traveledDistance = speed * elapsedSeconds;
            var totalDistance = Vector2.Distance(fromVector, toVector);
            var traveledPart = traveledDistance / totalDistance;
            var isFinish = traveledPart >= 1;

            if (isFinish)
            {
                return destination;
            }

            var movingVector = toVector - fromVector;
            var traveledVector = movingVector * traveledPart;
            var resultVector = fromVector + traveledVector;
            return new Location(resultVector.X, resultVector.Y);
        }

        private float GetSpeed()
        {
            var segment = map[location.X, location.Y];
            return segment.Speed;
        }

        public void FillRepositoryData(IMapObjectRepositoryData data)
        {
            data.Id = id;
            data.LocationX = location.X;
            data.LocationY = location.Y;
            data.DestinationX = destination.X;
            data.DestinationY = destination.Y;
        }
    }
}
