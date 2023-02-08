using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Väderkollen
{
    public class RegEx
    {
        static void Hämtadata()
        {
            string innetemperaturpattern = @"/(?<=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},Inne,).(\d.\d|.\d)/";
            string datumpattern = @"\d{2}-\d{2}(?<=-\d{2}-\d{2})";
            string luftfuktighetspattern = @" .(?<=\.\d,\d +).";
        }
        public static string RegExFunction(string pattern, string text)
        {

            Regex regex = new Regex(pattern);

            MatchCollection matches = regex.Matches(text);

            if  (matches != null)
            {
                foreach (Match match in matches)
                {
                    return match.ValueSpan.ToString();
                }
            }
            else 
                return "No matches";

            return
                   "No matches";
        }
   
    }
}
