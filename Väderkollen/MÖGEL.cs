using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Väderkollen
{

    internal class MÖGEL
    {
        public delegate void MyDelegate(List<string> data);
        static List<Action> MöGeLmEnU = new List<Action> { };
        public static void Run_Mold()
        {
            Console.WriteLine("Enter date you want to check for mold risk percentage");
            DateOnly userEntry = TryParseDateOnly();
            string date = userEntry.ToString("yyyy-MM-dd");

            MyDelegate del = CalculateMoldForDay;
            ReadLinesFromFile("{Program.path}", $"^{date}", del);
            Console.ReadLine();
        }
        public static void Run_Mold_All_Dates()
        {
            MyDelegate del = CalculateMoldForDay;
            DateTime startDate = new DateTime(2016, 05, 31);
            DateTime endDate = new DateTime(2017, 01, 10);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {

                ReadLinesFromFile($"{Program.path}", @"\b" + date.ToString("yyyy-MM-dd") + @"\b", del);
            }

            Console.ReadLine();
        }
        public static void CalculateMoldForDay(List<string> data)
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
                double riskForMold = (50 - Math.Abs(Convert.ToDouble(temperatures.Average()) - 20) + 50 - (100 - Convert.ToDouble(humidityValues.Average())));
                Console.WriteLine($"The risk for mold on {date} is {(int)riskForMold}%");
            }
            else Console.WriteLine(date);


        }
        private static void ReadLinesFromFile(string filePath, string pattern, MyDelegate process)

        {
            List<string> data = new List<string>();

            using (StreamReader reader = new StreamReader(Program.path + "tempdata5-medfel.txt"))
            {
                string line;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while ((line = reader.ReadLine()) != null)
                {
                    if (Regex.IsMatch(line, pattern))
                    {
                        data.Add(line);
                    }

                }
                stopwatch.Stop();
                Console.WriteLine($"It took {stopwatch.Elapsed} to complete the search");

                process(data);
            }

        }
        public static int CalculateRiskOfMoldPercentage(int humidity, int temperature)
        {
            int[] offset = new int[50];
            int[,] riskArray = new int[100, 50];
            for (int hum = 0; hum < 100; hum++)
            {
                for (int temp = 0; temp < 50; temp++)
                {
                    riskArray[hum, temp] = 50 - Math.Abs(temp - 20) + 50 - (100 - hum);

                }

            }
            for (int hum = 99; hum > 0; hum--)
            {
                for (int temp = 49; temp > 0; temp--)
                {


                    Console.Write($"[{riskArray[hum, temp]}]");
                }

                Console.WriteLine();

            }
            Console.ReadLine();

            int riskOfMoldPercentage = riskArray[humidity, temperature];

            return riskOfMoldPercentage;
        }
        public static DateOnly TryParseDateOnly()
        {
            DateOnly userEntry;
            bool success = false;
            while (!success)
            {
                success = DateOnly.TryParse(Console.ReadLine(), out userEntry);


                if (!success)
                {
                    IncorrectEntryMessage();
                    ClearAboveCursor(1);
                }
            }
            return userEntry;
        }
        public static void ClearAboveCursor(int linesToClear)
        {
            int cursorLeft;
            int cursorTop;
            (cursorLeft, cursorTop) = Console.GetCursorPosition();
            Console.SetCursorPosition(cursorLeft, cursorTop - linesToClear);
            Console.WriteLine();
            Console.WriteLine();
        }
        public static void IncorrectEntryMessage()
        {
            Console.WriteLine("Incorrect entry!");
            Console.WriteLine("Please try again");
            Thread.Sleep(1500);
            ClearAboveCursor(2);

        }
    }
}
