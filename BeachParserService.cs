using System;
using System.Net.Http;
using System.Threading.Tasks;
using BeachParser.DTO;
using BeachParser.Services;

namespace BeachParser
{
    public class BeachParserService
    {
        private readonly HttpClient _httpClient;
        private readonly WeatherService _weatherService;
        private readonly GeocodingService _geocodingService;
        private readonly FileService _fileService;

        public BeachParserService(string weatherApiKey)
        {
            _httpClient = new HttpClient();
            _weatherService = new WeatherService(_httpClient, weatherApiKey);
            _geocodingService = new GeocodingService(_httpClient);
            _fileService = new FileService();
        }

        public async Task<BeachData?> SearchBeachAsync(string beachName)
        {
            try
            {
                Console.WriteLine($"Поиск пляжа: {beachName}");
                Console.WriteLine("Получение координат...");

                var coordinates = await _geocodingService.GetBeachCoordinatesAsync(beachName);
                if (coordinates == null)
                {
                    Console.WriteLine("Пляж не найден");
                    Console.WriteLine("Попробуйте:");
                    Console.WriteLine("   • Более общее название");
                    Console.WriteLine("   • Добавить 'beach' или 'пляж'");
                    Console.WriteLine("   • Название на английском языке");
                    return null;
                }

                Console.WriteLine("Координаты получены");
                Console.WriteLine("Получение данных о погоде...");

                var weather = await _weatherService.GetWeatherDataAsync(coordinates.Latitude, coordinates.Longitude);
                if (weather == null)
                {
                    Console.WriteLine("Данные о погоде недоступны, используются примерные значения");
                    weather = new WeatherData();
                }

                Console.WriteLine("Данные о погоде получены");

                return new BeachData
                {
                    Name = beachName,
                    Country = coordinates.Country,
                    Latitude = coordinates.Latitude,
                    Longitude = coordinates.Longitude,
                    Address = coordinates.Address,
                    Weather = weather,
                    SearchDate = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }


        public void DisplayBeachData(BeachData beachData)
        {
            Console.Clear();
            Console.WriteLine("ПАРСЕР ИНФОРМАЦИИ О ПЛЯЖАХ");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{beachData.Name.ToUpper()}");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("МЕСТОПОЛОЖЕНИЕ:");
            Console.ResetColor();
            Console.WriteLine($"   Страна: {beachData.Country}");
            Console.WriteLine($"   Координаты: {beachData.Latitude:F4}, {beachData.Longitude:F4}");
            Console.WriteLine($"   Адрес: {beachData.Address}");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ПОГОДА:");
            Console.ResetColor();
            Console.WriteLine($"   Температура: {beachData.Weather.Temperature:F1}°C");
            Console.WriteLine($"   Влажность: {beachData.Weather.Humidity}%");
            Console.WriteLine($"   Скорость ветра: {beachData.Weather.WindSpeed:F1} м/с");
            Console.WriteLine($"   Описание: {beachData.Weather.Description}");
            Console.WriteLine($"   УФ-индекс: {beachData.Weather.UVIndex:F1}");
            Console.WriteLine($"   Давление: {beachData.Weather.Pressure:F0} гПа");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("РЕКОМЕНДАЦИИ:");
            Console.ResetColor();
            
            if (beachData.Weather.Temperature >= 25)
                Console.WriteLine("   Отличная погода для пляжа!");
            else if (beachData.Weather.Temperature >= 20)
                Console.WriteLine("   Хорошая погода для отдыха");
            else
                Console.WriteLine("   Прохладно для пляжа");

            if (beachData.Weather.WindSpeed > 8)
                Console.WriteLine("   Сильный ветер - будьте осторожны");
            else
                Console.WriteLine("   Умеренный ветер");

            if (beachData.Weather.UVIndex > 6)
                Console.WriteLine("   Высокий УФ-индекс - используйте солнцезащитный крем");
            else
                Console.WriteLine("   Умеренный УФ-индекс");

            Console.WriteLine();
            Console.WriteLine($"Дата поиска: {beachData.SearchDate:dd.MM.yyyy HH:mm}");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
        }

        public async Task SaveToFileAsync(BeachData beachData, string format)
        {
            await _fileService.SaveToFileAsync(beachData, format);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
