using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Väderkollen.Methods
{
    public class RunMethod
    {
        public static void Run_MoistureSpecific()
        {
            Get.Get_Moisture_Specific_Day(Program.DataList);

        }
        public static void Run_Temperature_Specific()
        {
            Get.Get_Temperature_Specific_Day(Program.DataList);

        }
        public static void Run_Least_Humid_Day_To_Most()
        {
            Get.Get_MostToLeastHumidDay(Program.DataList);

        }
        public static void Run_MonthlyTemperatureAndPrintToFile()
        {
            Get.Get_TemperatureMonthAndPrintToFile(Program.DataList);
        }
        public static void Run_Hottest_To_Coldest_Day()
        {
            Get.Get_WarmestToColdestDay(Program.DataList);

        }
        public static void Run_MonthlyHumidityAndPrintToFile()
        {
            Get.Get_MoistureMonthAndPrintToFile(Program.DataList);

        }
        public static void Run_MonthlyMoldRisk_AndPrintToFile()
        {
            Get.Get_MoldForMonth_And_Print_Mould_Formula(Program.DataList);
        }
        public static void Run_DailyMoldRisk()
        {
            Get.Get_Mold_Risk_Per_Day(Program.DataList);
        }
        public static void Metereologisk_Vinter_Och_Höst()
        {
            Get.Get_TemperaturesOchMetereologiskVinterOchHöst(Program.DataList);
        }
    }
}
