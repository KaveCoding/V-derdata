using System.IO;
using System.Text.RegularExpressions;


/*Utomhus
      Medeltemperatur och luftfuktighet per dag, valt datum, sökmöjlighet
      Sortering av varmast till kallaste dagen enligt medeltemperatur per dag
      sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag
      Sortering av minst till störst risk av mögel
      Datum för meteorologisk Höst
      Datum för meteologisk vinter
      */

/*Inomhus
Medeltemperatur per dag, valt datum
Sortering av varmast till kallaste dagen enligt medeltemperatur per dag
sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag
Sortering av minst till störst risk av mögel
*/


namespace Väderkollen
{
    internal class Program
    {
        public static string path = "../../../Files/";
        static void Main(string[] args)
        {
            ReadAll("tempdata5-medfel.txt");
        }

        public static void ReadAll(string filename)
        {
            using (StreamReader reader = new StreamReader(path + filename))
            {
                string fileContent = reader.ReadToEnd();
                Console.WriteLine(fileContent);
            }
        }    
        }
    }
