namespace BeachParser.DTO
{
    public class WeatherData
    {
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; } = string.Empty;
        public double UVIndex { get; set; }
        public double Pressure { get; set; }
    }
}
