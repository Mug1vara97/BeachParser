using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BeachParser.Services
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<dynamic> GetBeachCoordinatesAsync(string beachName)
        {
            try
            {
                var query = beachName + " beach";
                Console.WriteLine($"   Поиск: '{query}'");
                var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(query)}&format=json&limit=1&addressdetails=1";
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "BeachParser/1.0");

                var response = await _httpClient.GetStringAsync(url);
                var results = JsonSerializer.Deserialize<JsonElement[]>(response);

                Console.WriteLine($"   Найдено {results?.Length ?? 0} результатов");
                
                if (results != null && results.Length > 0)
                {
                    var result = results[0];
                    var displayName = result.GetProperty("display_name").GetString() ?? "Неизвестно";
                    Console.WriteLine($"   Результат: {displayName}");
                    
                    var latStr = result.GetProperty("lat").GetString() ?? "0";
                    var lonStr = result.GetProperty("lon").GetString() ?? "0";
                    
                    Console.WriteLine($"   Координаты: lat={latStr}, lon={lonStr}");
                    
                    return new
                    {
                        Latitude = double.Parse(latStr, System.Globalization.CultureInfo.InvariantCulture),
                        Longitude = double.Parse(lonStr, System.Globalization.CultureInfo.InvariantCulture),
                        Address = displayName,
                        Country = result.TryGetProperty("address", out var address) && 
                                 address.TryGetProperty("country", out var country) ? 
                                 country.GetString() ?? "Неизвестно" : "Неизвестно"
                    };
                }
                
                Console.WriteLine("   Результаты не найдены");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Ошибка: {ex.Message}");
                return null;
            }
        }
    }
}
