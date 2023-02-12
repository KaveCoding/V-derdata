using System.Globalization;

/*Utomhus
      Medeltemperatur och luftfuktighet per dag, valt datum, sökmöjlighet           klar
      Sortering av varmast till kallaste dagen enligt medeltemperatur per dag       klar
      sortering av torrast till fuktigaste dagen enligt medelluftfuktighet per dag  klar
      Sortering av minst till störst risk av mögel
      Datum för meteorologisk Höst                                                  klar
      Datum för meteologisk vinter                                                  klar
      */

/*Printmetoder
 
Medeltemperatur ute och inne, per månad Klar
Medelluftfuktighet inne och ute, per månad Klar
Medelmögelrisk inne och ute, per månad

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
        public static List<List<Data>> DataList = new List<List<Data>>();
        //Lista med alla menyalternativ som metoder, måste vara utan inparametrar.
        private static List<Action> Menu = new List<Action>()
            {
               Run_Least_Humid_Day_To_Most,
               Run_Hottest_To_Coldest_Day,
               Run_MoistureSpecific,
               Run_Temperature_Specific,
               Run_WarmestToColdestDay,
               Run_MonthlyHumidityAndPrintToFile,
               Run_MonthlyTemperatureAndPrintToFile,
               MÖGEL.Run_Mold,
               MÖGEL.Run_Mold_All_Dates

            };

        public static string path = "../../../Files/";
        static void Main(string[] args)
        {
            Console.WriteLine("Copying data");
            DataList = CopyDataToList("tempdata5-medfel.txt");
            Console.WriteLine("Data Copied! Press anything to continue");
            Console.ReadKey();
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

                Console.WriteLine($"[{i + 1}] {menuList[i].Method.Name.Replace("_", " ")}");
            }
            Console.WriteLine("[0]. Exit");
            int choice = TryParseReadLine(-1, menuList.Count);
            return choice;
        }
        public static void Run_MoistureSpecific()
        {
            Get_Moisture_Specific_Day(DataList);

        }
        public static void Run_Temperature_Specific()
        {
            Get_Temperature_Specific_Day(DataList);

        }
        public static void Run_Least_Humid_Day_To_Most()
        {
            GetMostToLeastHumidDay(DataList);

        }
        public static void Run_MonthlyTemperatureAndPrintToFile()
        {
            GetTemperatureMonthAndPrintToFile(DataList);
        }
        public static void Run_Hottest_To_Coldest_Day()
        {
            GetTemperaturesOchMetereologiskVinterOchHöst(DataList);

        }
        public static void Run_WarmestToColdestDay()
        {
            WarmestToColdestDay(DataList);

        }
        public static void Run_MonthlyHumidityAndPrintToFile()
        {
            GetMoistureMonthAndPrintToFile(DataList);

        }
        public static void WarmestToColdestDay(List<List<Data>> list)
        {

            List<Årstid> Höst = new List<Årstid>();

            List<Årstid> Vinter = new List<Årstid>();
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
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {group.Temperatur}");
            }



            ContinueMessage();

        }
        public static void GetTemperaturesOchMetereologiskVinterOchHöst(List<List<Data>> list)
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
            {
                Console.WriteLine($"Datum för meterologisk höst : {Höst[0].Dag} / {Höst[0].Månad} ");
                string newstring = $" Månad {Höst[0].Dag.ToString()} Medelfuktighet {Höst[1].Månad.ToString()} \n";
                File.AppendAllText(path + "Datum_för_meterologisk_höst.txt", newstring);
            }
               
                

            if (Vinter.Count() == 5)
                Console.WriteLine($"Datum för meterologisk vinter : {Vinter[0].Dag} / {Vinter[0].Månad} ");


            ContinueMessage();

        }
        public static void GetMostToLeastHumidDay(List<List<Data>> list)
        {

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    Månad = g.Key.Månad,
                    Dag = g.Key.Dag,
                    Fuktighet = (g.Average(s => s.Fuktighet)),
                }).OrderByDescending(g => g.Fuktighet).Reverse();

            Console.WriteLine("Ute: ");
            foreach (var group in groupbyMonthOutside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {Math.Round(group.Fuktighet)}");
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 Månad = g.Key.Månad,
                 Dag = g.Key.Dag,
                 Fuktighet = (g.Average(s => s.Fuktighet)),
             }).OrderByDescending(g => g.Fuktighet).Reverse();

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {Math.Round(group.Fuktighet)}");
            }

            ContinueMessage();
        }
        public static void GetMoistureMonthAndPrintToFile(List<List<Data>> list)
        {

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad }).Select(
                g => new
                {
                    Månad = g.Key.Månad,
                    Fuktighet = Math.Round(g.Average(s => s.Fuktighet)),
                    Summa = g.Sum(s=>s.Fuktighet),
                    Antal = g.Count()
                });

            Console.WriteLine("Ute per månad: ");
            foreach (var group in groupbyMonthOutside)
            {
                Console.WriteLine($"Månad: {group.Månad} Summa {group.Summa} / Antal {group.Antal} =  MedelFuktighet : {Math.Round(group.Fuktighet)} ");

                string newstring = $" Månad {group.Månad.ToString()} Medelfuktighet {group.Fuktighet.ToString()} \n";
                File.AppendAllText(path + "MedelFuktighetUte.txt", newstring);
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad}).Select(
             g => new
             {
                 Månad = g.Key.Månad,
                 Fuktighet = Math.Round(g.Average(s => s.Fuktighet)),
                 Summa = g.Sum(s => s.Fuktighet),
                 Antal = g.Count()
             });

            Console.WriteLine("Inne Per månad: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Månad: {group.Månad} Summa {group.Summa} / Antal {group.Antal} =  Medelfuktighet : {Math.Round(group.Fuktighet)} ");

                string newstring = $" Månad {group.Månad.ToString()} Medelfuktighet {group.Fuktighet.ToString()} \n";
                File.AppendAllText(path + "MedelFuktighetInne.txt", newstring);
            }

            ContinueMessage();
        }
        public static void GetTemperatureMonthAndPrintToFile(List<List<Data>> list)
        {
            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad }).Select(
                g => new
                {
                    Månad = g.Key.Månad,
                    Temperatur = Math.Round(Convert.ToDecimal(g.Average(s => s.Temperatur))),
                    Summa = g.Sum(s => s.Temperatur),
                    Antal = g.Count()
                }) ;

            Console.WriteLine("Ute per månad: ");
            foreach (var group in groupbyMonthOutside)
            {
                Console.WriteLine($"Månad: {group.Månad} Summa {group.Summa} / Antal {group.Antal} =  Medeltemperatur : {Math.Round(group.Temperatur)} ");

                string newstring = $" Månad {group.Månad.ToString()} Medeltemperatur {group.Temperatur.ToString()} \n";
                File.AppendAllText(path + "MedelTemperaturUte.txt", newstring);
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad}).Select(
             g => new
             {
                 Månad = g.Key.Månad,
                 Temperatur = Math.Round(Convert.ToDecimal(g.Average(s => s.Temperatur))),
                 Summa = g.Sum(s => s.Temperatur),
                 Antal = g.Count()
             });

            Console.WriteLine("Inne Per månad: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Månad: {group.Månad} Summa {Math.Round(Convert.ToDecimal(group.Summa))} / Antal {group.Antal} =  Medeltemperatur : {Math.Round(group.Temperatur)} ");
                string newstring = $" Månad {group.Månad.ToString()} Medeltemperatur {group.Temperatur.ToString()} \n";
                File.AppendAllText(path + "MedelTemperaturInne.txt", newstring);
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
                            Temperatur = Math.Round(Convert.ToDouble(temperatur, CultureInfo.InvariantCulture)),

                            Fuktighet = Math.Round(Convert.ToDouble(fuktighet))
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
        public static void CalculateMoldForDay(List<string> data)  //Elias version
        {
            List<float> temperatures = new List<float>();
            List<int> humidityValues = new List<int>();
            string date = RegEx.RegExFunction(@"\d{2}-\d{2}(?<=-\d{2}-\d{2})", data.DefaultIfEmpty("No date found").First());
            foreach (var line in data)
            {
                float temperature;
                string temp = RegEx.RegExFunction(@"(?<=\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},Inne,).(\d.\d|.\d)", line);
                bool successTemperature = float.TryParse(temp, NumberStyles.Any, CultureInfo.InvariantCulture, out temperature);
                int humidity;
                bool successHumidity = int.TryParse(RegEx.RegExFunction(@"\d+$", line), NumberStyles.Any, CultureInfo.InvariantCulture, out humidity);

                if (successTemperature && successHumidity)
                {
                    temperatures.Add(temperature);
                    humidityValues.Add(humidity);
                }
            }


            if (date != "No date found" && humidityValues.Count > 0 && temperatures.Count > 0)
            {
                double riskForMold = 100 - (Math.Abs(10 - Convert.ToDouble(temperatures.Average()))) - (Math.Abs(100 - Convert.ToDouble(humidityValues.Average())));
                Console.WriteLine($"The risk for mold on {date} is {(int)riskForMold}%");
            }
            else Console.WriteLine(date);


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

