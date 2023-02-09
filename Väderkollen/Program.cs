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

            SortByDays(CopyDataToList("tempdata5-medfel.txt"));

        }

   
        public static void SortByDays(List<Data> list)
        {
            List<Data> sortedByHumidity = new List<Data>();
            var groupbymonth = list.GroupBy(M => new { M.Månad, M.Dag} ).Select(
                g=> new
                {
                    Månad = g.Key.Månad,
                    Dag = g.Key.Dag,
                    Temperatur = (g.Average(s=>s.Temperatur))

                });;
            

            foreach(var group in groupbymonth)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {group.Temperatur}");
            }

            

        }

        public static List<Data> CopyDataToList(string filename)
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
                    var splittad_datum = datum.Split("-");
                    string månad = (splittad_datum[0]);
                    string månadutannoll = RegEx.RegExFunction(@"[^0]", månad);
                    string dag = splittad_datum[1];
                    string dagUtanNoll = RegEx.RegExFunction(@"[^0]", dag);



                    if (temperatur == "No matches") // minns ej
                        temperatur = null;
                    if (Convert.ToDouble(temperatur) > 100) //en temp som är 223
                        temperatur = null;
                    Data data = new Data()
                    {
                        Datum = datum,
                        Dag = int.Parse(dag),
                        Månad = int.Parse(månad),
                        Temperatur = Convert.ToDouble(temperatur),
                        UteEllerInne = uteEllerInne,
                        Fuktighet = Convert.ToDouble(fuktighet)
                    };
                    Datalist.Add(data);
                }
                Console.WriteLine("Datan är nu kopierad!");
                return Datalist;
            }
        }
    }
}
