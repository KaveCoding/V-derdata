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

            ReadLines("tempdata5-medfel.txt");
           
        }

        public static void ReadAll(string filename)
        {
            using (StreamReader reader = new StreamReader(path + filename))
            {
                string fileContent = reader.ReadToEnd();
                Console.WriteLine(fileContent);
            }
        }

        public static void ReadLines(string filename)
        {
            List<Data> Datalist = new List<Data>();
            using (StreamReader reader = new StreamReader(path + filename))
            {
                string[] lines = File.ReadAllLines(path+"tempdata5-medfel.txt");
                foreach(string line in lines)
                {
                    string temperatur = RegEx.RegExFunction(@"(?<=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},Ute,|Inne,).(\d.\d|.\d)", line);
                    string datum = RegEx.RegExFunction(@"\d{2}-\d{2}(?<=-\d{2}-\d{2})", line);
                    string fuktighet = RegEx.RegExFunction(@"\d+$", line);
                    string uteEllerInne = RegEx.RegExFunction(@"(Ute|Inne)", line);
                    if (temperatur == "No matches") // minns ej
                        temperatur = null;
                    if (Convert.ToDouble(temperatur) > 100) //en temp som är 223
                        temperatur = null;
                    Data data = new Data()
                    {
                        Datum = datum,
                        Temperatur = Convert.ToDouble(temperatur),
                        UteEllerInne = uteEllerInne,
                        Fuktighet = Convert.ToDouble(fuktighet)
                    };
                    Datalist.Add(data);
                }
                Console.WriteLine("Datan är nu flyttad!");
            }
        }
    }
}
