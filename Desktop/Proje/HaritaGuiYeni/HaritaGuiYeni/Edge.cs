using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaritaGuiYeni
{
    class Edge
    {
        public String DataBaseIdSourceLok { get; set; }
        public String DataBaseIdDestinationLok { get; set; }
        public lokasyon SourceLok { get; set; }
        public lokasyon DestinationLok { get; set; }
        public int Source { get; set; }
        public int Destination { get; set; }
        public int Weight { get; set; }

    }
}
