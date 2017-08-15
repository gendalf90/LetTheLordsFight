using MapDomain.Common;
using MapDomain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapDomain.ValueObjects
{
    public class Map
    {
        private readonly float segmentSize;
        private readonly SegmentType[,] segmentTypes;
        private readonly Dictionary<SegmentType, float> segmentsSpeed;

        public Map(MapCreateData createData)
        {
            segmentSize = createData.SegmentSize;
            segmentTypes = createData.Types;
            segmentsSpeed = createData.SegmentsSpeed;
        }

        public Segment this[float x, float y]
        {
            get
            {
                var location = GetIJByXY(x, y);
                return this[location.I, location.J];
            }
        }

        public Segment this[int i, int j]
        {
            get
            {
                ValidateIJ(i, j);
                return GetByIJ(i, j);
            }
        }

        private Segment GetByIJ(int i, int j)
        {
            var leftUpLocation = new Location(i * segmentSize, j * segmentSize);
            var rightDownLocation = new Location((i + 1) * segmentSize, (j + 1) * segmentSize);
            var type = segmentTypes[i, j];
            var speed = segmentsSpeed[type];
            return new Segment(leftUpLocation, rightDownLocation, type, speed);
        }

        private void ValidateIJ(int i, int j)
        {
            if (i < 0 || i >= SegmentsHeight || j < 0 || j >= SegmentsWidth)
            {
                throw new NotFoundException();
            }
        }

        private (int I, int J) GetIJByXY(float x, float y)
        {
            var i = (int)(y / segmentSize);
            var j = (int)(x / segmentSize);
            return (I: i, J: j);
        }

        public int SegmentsWidth
        {
            get => segmentTypes.GetLength(1);
        }

        public int SegmentsHeight
        {
            get => segmentTypes.GetLength(0);
        }

        public float Width
        {
            get => SegmentsWidth * segmentSize;
        }

        public float Height
        {
            get => SegmentsHeight * segmentSize;
        }
    }
}
