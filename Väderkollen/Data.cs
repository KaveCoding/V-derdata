namespace Väderkollen
{
    public class Data
    {
        public string Datum { get; set; }
        public int Månad { get; set; }
        public int Dag { get; set; }
        public double? Temperatur { get; set; }
        //public string UteEllerInne { get; set; }
        public double Fuktighet { get; set; }
        public double? Mögelrisk { get; set; }

    }
}
