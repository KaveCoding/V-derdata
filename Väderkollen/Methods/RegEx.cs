using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Väderkollen.Methods
{
    public class RegEx
    {
        public static string RegExFunction(string pattern, string text)
        {
            Regex regex = new Regex(pattern);

            MatchCollection matches = regex.Matches(text);

            if (matches != null)
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
