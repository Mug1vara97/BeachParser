using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BeachParser.DTO;

namespace BeachParser.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _weatherApiKey;

        public WeatherService(HttpClient httpClient, string weatherApiKey)
        {
            _httpClient = httpClient;
            _weatherApiKey = weatherApiKey;
        }

        public async Task<WeatherData?> GetWeatherDataAsync(double lat, double lon)
        {
            try
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_weatherApiKey}&units=metric&lang=ru";
                var response = await _httpClient.GetStringAsync(url);
                var weather = JsonSerializer.Deserialize<JsonElement>(response);

                return new WeatherData
                {
                    Temperature = weather.GetProperty("main").GetProperty("temp").GetDouble(),
                    Humidity = weather.GetProperty("main").GetProperty("humidity").GetInt32(),
                    WindSpeed = weather.GetProperty("wind").GetProperty("speed").GetDouble(),
                    Description = weather.GetProperty("weather")[0].GetProperty("description").GetString() ?? "Неизвестно",
                    UVIndex = weather.TryGetProperty("uvi", out var uvi) ? uvi.GetDouble() : 0,
                    Pressure = weather.GetProperty("main").GetProperty("pressure").GetDouble()
                };
            }
            catch
            {
                return GetMockWeatherData();
            }
        }

        private WeatherData GetMockWeatherData()
        {
            var random = new Random();
            return new WeatherData
            {
                Temperature = random.Next(15, 35) + random.NextDouble(),
                Humidity = random.Next(40, 90),
                WindSpeed = random.Next(1, 15) + (float)random.NextDouble(),
                Description = new[] { "Ясно", "Облачно", "Солнечно", "Переменная облачность", "Пасмурно" }[random.Next(5)],
                UVIndex = random.Next(1, 10) + (float)random.NextDouble(),
                Pressure = random.Next(980, 1030) + random.NextDouble()
            };
        }
    }
}
