using System.Globalization;
/*Utomhus
      Medeltemperatur och luftfuktighet per dag, valt datum, sökmöjlighet           klar
      Sortering av varmast till kallaste dagen enligt medeltemperatur per dag       klar
      sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag  klar
      Sortering av minst till störst risk av mögel
      Datum för meteorologisk Höst
      Datum för meteologisk vinter
      */

/*Inomhus
Medeltemperatur per dag, valt datum                                                 klar
Sortering av varmast till kallaste dagen enligt medeltemperatur per dag             klar
sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag        klar
Sortering av minst till störst risk av mögel
*/
namespace Väderkollen
{
    internal class Program
    {
        //Lista med alla menyalternativ som metoder, måste vara utan inparametrar.
        private static List<Action> Menu = new List<Action>()
            {
               Run_Moisture,
               Run_Temperatures,
               Run_MoistureSpecific,
               Run_Temperature_Specific
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


        public static void Run_MoistureSpecific()
        {
            Get_Moisture_Specific_Day(CopyDataToList("tempdata5-medfel.txt"));

        }
        public static void Run_Temperature_Specific()
        {
            Get_Temperature_Specific_Day(CopyDataToList("tempdata5-medfel.txt"));

        }
        public static void Run_Moisture()
        {
            GetMoisture(CopyDataToList("tempdata5-medfel.txt"));

        }
        public static void Run_Temperatures()
        {
            GetTemperatures(CopyDataToList("tempdata5-medfel.txt"));

        }


        public static void GetTemperatures(List<List<Data>> list)
        {

            List<Årstid> Höst = new List<Årstid>();

            List<Årstid> Vinter = new List<Årstid>();
            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    Månad = g.Key.Månad,
                    Dag = g.Key.Dag,
                    Temperatur = (g.Average(s => s.Temperatur)),
                });

            Console.WriteLine("Ute: ");
            foreach (var group in groupbyMonthOutside)
            {
                if (group.Temperatur < 10 && Höst.Count <= 4)
                    Höst.Add(new Årstid() { Dag = group.Dag, Månad = group.Månad });
                else
                {
                    if (Höst.Count != 5)
                        Höst.Clear();
                }

               

                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {group.Temperatur}");
                string newstring =$"Dag {group.Dag.ToString()} Månad {group.Månad.ToString()} Medeltemperatur {group.Temperatur.ToString()} \n";
                File.AppendAllText(path + "Medeltemperaturer.txt",newstring);

                if (group.Temperatur < 0 && Vinter.Count <= 4)
                    Vinter.Add(new Årstid() { Dag = group.Dag, Månad = group.Månad });
                else
                {
                    if (Vinter.Count != 5)
                        Vinter.Clear();
                }
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 Månad = g.Key.Månad,
                 Dag = g.Key.Dag,
                 Temperatur = (g.Average(s => s.Temperatur)),
             });

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {group.Temperatur}");
            }

            if (Höst.Count() == 5)
                Console.WriteLine($"Datum för meterologisk höst : {Höst[0].Dag} / {Höst[0].Månad} ");

            if (Vinter.Count() == 5)
                Console.WriteLine($"Datum för meterologisk vinter : {Vinter[0].Dag} / {Vinter[0].Månad} ");
       

            ContinueMessage();

        }

        public static void GetMoisture(List<List<Data>> list)
        {

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    Månad = g.Key.Månad,
                    Dag = g.Key.Dag,
                    Fuktighet = (g.Average(s => s.Fuktighet)),
                }).OrderByDescending(g => g.Fuktighet);

            Console.WriteLine("Ute: ");
            foreach (var group in groupbyMonthOutside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {group.Fuktighet}");
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 Månad = g.Key.Månad,
                 Dag = g.Key.Dag,
                 Fuktighet = (g.Average(s => s.Fuktighet)),
             }).OrderByDescending(g => g.Fuktighet);

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {group.Fuktighet}");
            }




            ContinueMessage();
        }
        public static void Get_Moisture_Specific_Day(List<List<Data>> list)
        {

            Console.WriteLine("Skriv in dag");
            var dag = TryParseReadLine(1, 31);
            Console.WriteLine("Skriv in Månad");
            var månad = TryParseReadLine(1, 12);

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    Månad = g.Key.Månad,
                    Dag = g.Key.Dag,
                    Fuktighet = (g.Average(s => s.Fuktighet)),
                }).OrderByDescending(g => g.Fuktighet);

            Console.WriteLine("Ute: ");


            foreach (var group in groupbyMonthOutside)
            {
                if (group.Dag == dag && group.Månad == månad)
                    Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Fuktighet : {group.Fuktighet}");
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 Månad = g.Key.Månad,
                 Dag = g.Key.Dag,
                 Fuktighet = (g.Average(s => s.Fuktighet)),
             }).OrderByDescending(g => g.Fuktighet);

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                if (group.Dag == dag && group.Månad == månad)
                    Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Fuktighet : {group.Fuktighet}");
            }
            ContinueMessage();
        }
        public static void Get_Temperature_Specific_Day(List<List<Data>> list)
        {
            Console.WriteLine("Skriv in dag");
            var dag = TryParseReadLine(1, 31);
            Console.WriteLine("Skriv in Månad");
            var månad = TryParseReadLine(1, 12);

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    Månad = g.Key.Månad,
                    Dag = g.Key.Dag,
                    Temperatur = (g.Average(s => s.Temperatur)),
                }).OrderByDescending(g => g.Temperatur);

            Console.WriteLine("Ute: ");


            foreach (var group in groupbyMonthOutside)
            {
                if (group.Dag == dag && group.Månad == månad)
                    Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {group.Temperatur}");
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 Månad = g.Key.Månad,
                 Dag = g.Key.Dag,
                 Temperatur = (g.Average(s => s.Temperatur)),
             }).OrderByDescending(g => g.Temperatur);

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                if (group.Dag == dag && group.Månad == månad)
                    Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {group.Temperatur}");
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
                    string dag = splittad_datum[1];


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

