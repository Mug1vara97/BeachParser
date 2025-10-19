namespace BeachParser.DTO
{
    public class BeachData
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; } = string.Empty;
        public WeatherData Weather { get; set; } = new WeatherData();
        public DateTime SearchDate { get; set; }
    }
}
