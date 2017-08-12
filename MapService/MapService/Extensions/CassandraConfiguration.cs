using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapService.Extensions
{
    class CassandraConfiguration
    {
        public string[] Nodes { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
