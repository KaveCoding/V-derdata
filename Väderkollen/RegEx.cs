using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Väderkollen
{
    internal class RegEx
    {
        static void Hämtadata()
        {
            string temperaturpattern = @"/(?<=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},Ute,).(\d.\d|.\d)/";
            string datumpattern = @"\d{2}-\d{2}(?<=-\d{2}-\d{2})/";

        }
    }
}
