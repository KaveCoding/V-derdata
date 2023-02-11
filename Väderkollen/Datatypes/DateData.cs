namespace Väderkollen.Datatypes
{
    internal class DateData
    {
        public DateOnly Date { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int RiskOfMouldPercentage { get; set; }
        public DateData(string date, double temperature, double humidity)
        {
            Date = DateOnly.Parse(date);
            Temperature = temperature;
            Humidity = humidity;

        }
        public DateData()
        {

        }



    }
}
