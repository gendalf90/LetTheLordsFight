using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Options
{
    public class Segments
    {
        public int Size { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public byte[] Types { get; set; }

        public float[] Speed { get; set; }
    }
}
