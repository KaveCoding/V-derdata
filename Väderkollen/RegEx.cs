using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Väderkollen
{
    internal class RegEx
    {
        static void Hämtadata()
        {
            string innetemperaturpattern = @"/(?<=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},Inne,).(\d.\d|.\d)/";
            string datumpattern = @"\d{2}-\d{2}(?<=-\d{2}-\d{2})";

        }
        public static void RegExFunction(string pattern, string text)
        {
            Regex regex = new Regex(pattern);

            MatchCollection matches = regex.Matches(text);

            Console.WriteLine("Antal matchingar: " + matches.Count);

            foreach (Match match in matches)
            {
                Console.WriteLine(match.Value + " på plats " + match.Index + " med längden " + match.Length);
            }
        }
   
    }
}
