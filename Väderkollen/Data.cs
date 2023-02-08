using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Väderkollen
{
    public class Data
    {
        public string Datum { get; set; }
        public double? Temperatur { get; set; }
        public string UteEllerInne { get; set; }
        public double Fuktighet { get; set; }

    }
}
