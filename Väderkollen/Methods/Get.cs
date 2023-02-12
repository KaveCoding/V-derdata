using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Väderkollen.Datatypes;

namespace Väderkollen.Methods
{
    public class Get
    {
        public static void Get_WarmestToColdestDay(List<List<Data>> list)
        {

            List<Årstid> Höst = new List<Årstid>();

            List<Årstid> Vinter = new List<Årstid>();
            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    g.Key.Månad,
                    g.Key.Dag,
                    Temperatur = g.Average(s => s.Temperatur),
                }).OrderByDescending(g => g.Temperatur);

            Console.WriteLine("Ute: ");
            foreach (var group in groupbyMonthOutside)
            {

                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {Math.Round(Convert.ToDecimal(group.Temperatur))}");
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 g.Key.Månad,
                 g.Key.Dag,
                 Temperatur = g.Average(s => s.Temperatur),
             }).OrderByDescending(g => g.Temperatur);

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {Math.Round(Convert.ToDecimal(group.Temperatur))}");
            }



            Program.ContinueMessage();

        }
        public static void Get_TemperaturesOchMetereologiskVinterOchHöst(List<List<Data>> list)
        {

            List<Årstid> Höst = new List<Årstid>();

            List<Årstid> Vinter = new List<Årstid>();
            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    g.Key.Månad,
                    g.Key.Dag,
                    Temperatur = g.Average(s => s.Temperatur),
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
                 g.Key.Månad,
                 g.Key.Dag,
                 Temperatur = g.Average(s => s.Temperatur),
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
                File.AppendAllText(Program.path + "Datum_för_meterologisk_höst.txt", newstring);
            }



            if (Vinter.Count() == 5)
                Console.WriteLine($"Datum för meterologisk vinter : {Vinter[0].Dag} / {Vinter[0].Månad} ");


            Program.ContinueMessage();

        }
        public static void Get_MostToLeastHumidDay(List<List<Data>> list)
        {

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    g.Key.Månad,
                    g.Key.Dag,
                    Fuktighet = g.Average(s => s.Fuktighet),
                }).OrderByDescending(g => g.Fuktighet).Reverse();

            Console.WriteLine("Ute: ");
            foreach (var group in groupbyMonthOutside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {Math.Round(group.Fuktighet)}");
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 g.Key.Månad,
                 g.Key.Dag,
                 Fuktighet = g.Average(s => s.Fuktighet),
             }).OrderByDescending(g => g.Fuktighet).Reverse();

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Temperatur : {Math.Round(group.Fuktighet)}");
            }

            Program.ContinueMessage();
        }
        public static void Get_MoistureMonthAndPrintToFile(List<List<Data>> list)
        {

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad }).Select(
                g => new
                {
                    g.Key.Månad,
                    Fuktighet = Math.Round(g.Average(s => s.Fuktighet)),
                    Summa = g.Sum(s => s.Fuktighet),
                    Antal = g.Count()
                });

            Console.WriteLine("Ute per månad: ");
            foreach (var group in groupbyMonthOutside)
            {
                Console.WriteLine($"Månad: {group.Månad} Summa {group.Summa} / Antal {group.Antal} =  MedelFuktighet : {Math.Round(group.Fuktighet)} ");

                string newstring = $" Månad {group.Månad.ToString()} Medelfuktighet {group.Fuktighet.ToString()} \n";
                File.AppendAllText(Program.path + "MedelFuktighetUte.txt", newstring);
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad }).Select(
             g => new
             {
                 g.Key.Månad,
                 Fuktighet = Math.Round(g.Average(s => s.Fuktighet)),
                 Summa = g.Sum(s => s.Fuktighet),
                 Antal = g.Count()
             });

            Console.WriteLine("Inne Per månad: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Månad: {group.Månad} Summa {group.Summa} / Antal {group.Antal} =  Medelfuktighet : {Math.Round(group.Fuktighet)} ");

                string newstring = $" Månad {group.Månad.ToString()} Medelfuktighet {group.Fuktighet.ToString()} \n";
                File.AppendAllText(Program.path + "MedelFuktighetInne.txt", newstring);
            }

            Program.ContinueMessage();
        }
        public static void Get_TemperatureMonthAndPrintToFile(List<List<Data>> list)
        {
            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad }).Select(
                g => new
                {
                    g.Key.Månad,
                    Temperatur = Math.Round(Convert.ToDecimal(g.Average(s => s.Temperatur))),
                    Summa = g.Sum(s => s.Temperatur),
                    Antal = g.Count()
                });

            Console.WriteLine("Ute per månad: ");
            foreach (var group in groupbyMonthOutside)
            {
                Console.WriteLine($"Månad: {group.Månad} Summa {group.Summa} / Antal {group.Antal} =  Medeltemperatur : {Math.Round(group.Temperatur)} ");

                string newstring = $" Månad {group.Månad.ToString()} Medeltemperatur {group.Temperatur.ToString()} \n";
                File.AppendAllText(Program.path + "MedelTemperaturUte.txt", newstring);
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad }).Select(
             g => new
             {
                 g.Key.Månad,
                 Temperatur = Math.Round(Convert.ToDecimal(g.Average(s => s.Temperatur))),
                 Summa = g.Sum(s => s.Temperatur),
                 Antal = g.Count()
             });

            Console.WriteLine("Inne Per månad: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Månad: {group.Månad} Summa {Math.Round(Convert.ToDecimal(group.Summa))} / Antal {group.Antal} =  Medeltemperatur : {Math.Round(group.Temperatur)} ");
                string newstring = $" Månad {group.Månad.ToString()} Medeltemperatur {group.Temperatur.ToString()} \n";
                File.AppendAllText(Program.path + "MedelTemperaturInne.txt", newstring);
            }

            Program.ContinueMessage();
        }
        public static void Get_Moisture_Specific_Day(List<List<Data>> list)
        {

            Console.WriteLine("Skriv in dag");
            var dag = Program.TryParseReadLine(1, 31);
            Console.WriteLine("Skriv in Månad");
            var månad = Program.TryParseReadLine(1, 12);

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    g.Key.Månad,
                    g.Key.Dag,
                    Fuktighet = g.Average(s => s.Fuktighet),
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
                 g.Key.Månad,
                 g.Key.Dag,
                 Fuktighet = g.Average(s => s.Fuktighet),
             }).OrderByDescending(g => g.Fuktighet);

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                if (group.Dag == dag && group.Månad == månad)
                    Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Fuktighet : {group.Fuktighet}");
            }
            Program.ContinueMessage();
        }
        public static void Get_Temperature_Specific_Day(List<List<Data>> list)
        {
            Console.WriteLine("Skriv in dag");
            var dag = Program.TryParseReadLine(1, 31);
            Console.WriteLine("Skriv in Månad");
            var månad = Program.TryParseReadLine(1, 12);

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
                g => new
                {
                    g.Key.Månad,
                    g.Key.Dag,
                    Temperatur = g.Average(s => s.Temperatur),
                }).OrderByDescending(g => g.Temperatur);

            Console.WriteLine("Ute: ");


            foreach (var group in groupbyMonthOutside)
            {
                if (group.Dag == dag && group.Månad == månad)
                    Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Medeltemperatur : {Math.Round(Convert.ToDecimal(group.Temperatur))}");
            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 g.Key.Månad,
                 g.Key.Dag,
                 Temperatur = g.Average(s => s.Temperatur),
             }).OrderByDescending(g => g.Temperatur);

            Console.WriteLine("Inne: ");
            foreach (var group in groupbyMonthInside)
            {
                if (group.Dag == dag && group.Månad == månad)
                    Console.WriteLine($"Dag : {group.Dag} Månad: {group.Månad} Medeltemperatur : {Math.Round(Convert.ToDecimal(group.Temperatur))}");
            }
            Program.ContinueMessage();
        }
        public static void Get_Mold_Risk_Per_Day(List<List<Data>> list)
        {

            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad, M.Dag }).Select(
               g => new
               {
                   g.Key.Månad,
                   g.Key.Dag,
                   Temperatur = Math.Round(Convert.ToDecimal(g.Average(s => s.Temperatur))),
                   Fuktighet = Math.Round(Convert.ToDecimal(g.Average(s => s.Fuktighet))),
                   Mögelrisk = 100 - Math.Abs(30 - Convert.ToDecimal(g.Average(s => s.Temperatur))) - Math.Abs(Convert.ToDecimal(100 - g.Average(s => s.Fuktighet)))
               }).OrderBy(s => s.Mögelrisk);

            Console.WriteLine("Ute per dag: ");


            foreach (var group in groupbyMonthOutside)
            {

                Console.WriteLine($"Day {group.Dag} Month {group.Månad} Temperatur : {group.Temperatur} Fuktighet : {group.Fuktighet} Mögelrisk :{Math.Round(group.Mögelrisk)}");

            }

            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad, M.Dag }).Select(
             g => new
             {
                 g.Key.Månad,
                 g.Key.Dag,
                 Temperatur = Math.Round(Convert.ToDecimal(g.Average(s => s.Temperatur))),
                 Fuktighet = Math.Round(Convert.ToDecimal(g.Average(s => s.Fuktighet))),
                 Mögelrisk = 100 - Math.Abs(30 - Convert.ToDecimal(g.Average(s => s.Temperatur))) - Math.Abs(Convert.ToDecimal(100 - g.Average(s => s.Fuktighet)))

             }).OrderBy(s => s.Mögelrisk);

            Console.WriteLine("Inne per dag: ");
            foreach (var group in groupbyMonthInside)
            {
                Console.WriteLine($"Day {group.Dag} Month {group.Månad} Temperatur : {group.Temperatur} Fuktighet : {group.Fuktighet} Mögelrisk :{Math.Round(group.Mögelrisk)}");
            }

            Program.ContinueMessage();
        }
        public static void Get_MoldForMonth_And_Print_Mould_Formula(List<List<Data>> list)
        {
            var groupbyMonthOutside = list[0].GroupBy(M => new { M.Månad }).Select(
               g => new
               {
                   g.Key.Månad,
                   Temperatur = Math.Round(Convert.ToDecimal(g.Average(s => s.Temperatur))),
                   Fuktighet = Math.Round(Convert.ToDecimal(g.Average(s => s.Fuktighet))),

               });

            Console.WriteLine("Ute per månad: ");
            foreach (var group in groupbyMonthOutside)
            {

                double riskForMold = 100 - Math.Abs(30 - Convert.ToDouble(group.Temperatur)) - Math.Abs(100 - Convert.ToDouble(group.Fuktighet));
                Console.WriteLine($"Månad: {group.Månad} Medelfuktighet {group.Fuktighet} Medeltemperatur {Math.Round(group.Temperatur)}");
                Console.WriteLine($"The risk for mold on månad {group.Månad} is {(int)riskForMold}%");

                string newstring = $" Månad: {group.Månad} Mögelrisk: {riskForMold} \n";
                File.AppendAllText(Program.path + "Mögelrisk_Per_Månad_Ute.txt", newstring);
            }
            var groupbyMonthInside = list[1].GroupBy(M => new { M.Månad }).Select(
             g => new
             {
                 g.Key.Månad,
                 Temperatur = Math.Round(Convert.ToDecimal(g.Average(s => s.Temperatur))),
                 Fuktighet = Math.Round(Convert.ToDecimal(g.Average(s => s.Fuktighet))),

             });

            Console.WriteLine("Inne per månad: ");
            foreach (var group in groupbyMonthInside)
            {

                double riskForMold = 100 - Math.Abs(30 - Convert.ToDouble(group.Temperatur)) - Math.Abs(100 - Convert.ToDouble(group.Fuktighet));
                Console.WriteLine($"Månad: {group.Månad} Medelfuktighet {group.Fuktighet} Medeltemperatur {Math.Round(group.Temperatur)}");
                Console.WriteLine($"The risk for mold on månad {group.Månad} is {(int)riskForMold}%");

                string newstring = $" Månad: {group.Månad} Mögelrisk: {riskForMold} \n";
                File.AppendAllText(Program.path + "Mögelrisk_per_Månad_inne.txt", newstring);
            }

            File.AppendAllText(Program.path + "Mögelformel.txt", $" 100 - abs(30-medeltemperatur) - abs(100-fuktighet)");

            Program.ContinueMessage();

        }
    }
}
