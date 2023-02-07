using System.IO;

namespace Väderkollen
{
    internal class Program
    {
        public static string path = "../Bin/";
        static void Main(string[] args)
        {

          
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


        static void ReadLines(string filename)
            {
                using (StreamReader reader = new StreamReader(path + filename))
                {


                    int lineNumber = 0;
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        lineNumber++;
                        Console.WriteLine("Rad " + lineNumber + " " + line);
                        line = reader.ReadLine();
                    }


                }
            }
            Console.WriteLine("Hello, World!");
        }
    }
}