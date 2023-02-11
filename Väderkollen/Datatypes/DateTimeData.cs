namespace Väderkollen.Datatypes
{
    internal class DateTimeData
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public DateTimeData(string date, double temperature, double humidity)
        {
            Date = DateTime.Parse(date);
            Temperature = temperature;
            Humidity = humidity;
        }
        public DateTimeData()
        {

        }
    }
}
