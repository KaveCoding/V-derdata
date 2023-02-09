using System.Globalization;
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
        //Lista med alla menyalternativ som metoder, måste vara utan inparametrar.
        private static List<Action> Menu = new List<Action>()
            {
               Run_Template
            };

        public static string path = "../../../Files/";
        static void Main(string[] args)
        {

            Run();

        }
        public static void Run()
        {
            int choice;
            bool loop = true;
            while (loop)
            {
                choice = PrintMenu(Menu, "Menu");
                if (choice == 0)
                {
                    loop = false;
                    break;
                }
                else
                {
                    Menu[choice - 1]();
                }
            }
        }
        public static int PrintMenu(List<Action> menuList, string header)
        {
            Console.Clear();
            Console.SetCursorPosition(Console.BufferWidth / 2, 3);
            Console.WriteLine(header);
            Console.WriteLine();
            Console.WriteLine("Choose an option:");
            for (int i = 0; i < menuList.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {menuList[i].Method.Name}");
            }
            Console.WriteLine("[0]. Exit");
            int choice = TryParseReadLine(-1, menuList.Count);
            return choice;
        }
      
        public static void Run_Template()
        {
            Template(CopyDataToList("tempdata5-medfel.txt"));
            
        }
       
        public static void Template(List<List<Data>> list)
        {

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    Månad = g.Key.Månad,
                    Dag = g.Key.Dag,
                    Fuktighet = (g.Average(s => s.Fuktighet)),
                });

            Console.WriteLine("Ute: ");
            foreach (var group in groupbyMonthOutside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Fuktighet : {group.Fuktighet}");
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 Månad = g.Key.Månad,
                 Dag = g.Key.Dag,
                 Fuktighet = (g.Average(s => s.Fuktighet)),
             });

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Fuktighet : {group.Fuktighet}");
            }
            ContinueMessage();
        }

        public static List<List<Data>> CopyDataToList(string filename)
        {
            List<List<Data>> DataList = new List<List<Data>>();
            List<Data> UteDatalist = new List<Data>();
            List<Data> InneDataList = new List<Data>();
            using (StreamReader reader = new StreamReader(path + filename))
            {
                string[] lines = File.ReadAllLines(path + "tempdata5-medfel.txt");
                foreach (string line in lines)
                {
                    string temperatur = RegEx.RegExFunction(@"(?<=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},Ute,|Inne,).(\d.\d|.\d)", line);
                    string datum = RegEx.RegExFunction(@"\d{2}-\d{2}(?<=-\d{2}-\d{2})", line);
                    string fuktighet = RegEx.RegExFunction(@"\d+$", line);
                    string uteEllerInne = RegEx.RegExFunction(@"(Ute|Inne)", line);
                    var splittad_datum = datum.Split("-");
                    string månad = (splittad_datum[0]);
                    //string månadutannoll = RegEx.RegExFunction(@"[^0]", månad);
                    string dag = splittad_datum[1];
                    //string dagUtanNoll = RegEx.RegExFunction(@"[^0]", dag);



                    if (temperatur == "No matches") // minns ej
                        temperatur = null;
                    if (Convert.ToDouble(temperatur, CultureInfo.InvariantCulture) > 100) //en temp som är 223
                        temperatur = null;
                    if (uteEllerInne == "Ute")
                    {
                        Data data = new Data()
                        {
                            Datum = datum,
                            Dag = int.Parse(dag),
                            Månad = int.Parse(månad),
                            Temperatur = Convert.ToDouble(temperatur, CultureInfo.InvariantCulture),

                            Fuktighet = Convert.ToDouble(fuktighet)
                        };
                        UteDatalist.Add(data);
                    }
                    else if (uteEllerInne == "Inne")
                    {
                        Data data = new Data()
                        {
                            Datum = datum,
                            Dag = int.Parse(dag),
                            Månad = int.Parse(månad),
                            Temperatur = Convert.ToDouble(temperatur, CultureInfo.InvariantCulture),
                            Fuktighet = Convert.ToDouble(fuktighet, CultureInfo.InvariantCulture)
                        };
                        InneDataList.Add(data);
                    }
                }
                DataList.Add(UteDatalist);
                DataList.Add(InneDataList);
                Console.WriteLine("Datan är nu kopierad!");
                return DataList;
            }
        }

        public static int TryParseReadLine(int spanLow, int spanHigh)
        {
            int key = 0;
            bool success = false;
            while (!success)
            {
                Console.WriteLine($"Enter choice between {spanLow} and {spanHigh}");
                success = int.TryParse(Console.ReadLine(), out key);
                if (key < spanLow && key > spanHigh)
                {
                    success = false;
                }
                if (!success)
                {
                    Console.WriteLine("Incorrect entry!");
                    Console.WriteLine("Please try again");
                    Thread.Sleep(2000);
                    int cursorLeft;
                    int cursorTop;
                    (cursorLeft, cursorTop) = Console.GetCursorPosition();
                    Console.SetCursorPosition(cursorLeft, cursorTop - 2);
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
            return key;
        }
        public static void ContinueMessage()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }


    }
}
