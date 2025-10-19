using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BeachParser.DTO;

namespace BeachParser.Services
{
    public class FileService
    {
        public async Task SaveToFileAsync(BeachData beachData, string format)
        {
            try
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var fileName = $"beach_data_{beachData.Name.Replace(" ", "_")}_{timestamp}";

                if (format.ToLower() == "json")
                {
                    fileName += ".json";
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    var json = JsonSerializer.Serialize(beachData, options);
                    await File.WriteAllTextAsync(fileName, json, Encoding.UTF8);
                }
                else if (format.ToLower() == "txt")
                {
                    fileName += ".txt";
                    var sb = new StringBuilder();
                    sb.AppendLine("ИНФОРМАЦИЯ О ПЛЯЖЕ");
                    sb.AppendLine("═══════════════════════════════════════════════════════════");
                    sb.AppendLine();
                    sb.AppendLine($"{beachData.Name.ToUpper()}");
                    sb.AppendLine();
                    sb.AppendLine("МЕСТОПОЛОЖЕНИЕ:");
                    sb.AppendLine($"   Страна: {beachData.Country}");
                    sb.AppendLine($"   Координаты: {beachData.Latitude:F4}, {beachData.Longitude:F4}");
                    sb.AppendLine($"   Адрес: {beachData.Address}");
                    sb.AppendLine();
                    sb.AppendLine("ПОГОДА:");
                    sb.AppendLine($"   Температура: {beachData.Weather.Temperature:F1}°C");
                    sb.AppendLine($"   Влажность: {beachData.Weather.Humidity}%");
                    sb.AppendLine($"   Скорость ветра: {beachData.Weather.WindSpeed:F1} м/с");
                    sb.AppendLine($"   Описание: {beachData.Weather.Description}");
                    sb.AppendLine($"   УФ-индекс: {beachData.Weather.UVIndex:F1}");
                    sb.AppendLine($"   Давление: {beachData.Weather.Pressure:F0} гПа");
                    sb.AppendLine();
                    sb.AppendLine($"Дата поиска: {beachData.SearchDate:dd.MM.yyyy HH:mm}");
                    sb.AppendLine("═══════════════════════════════════════════════════════════");
                    
                    await File.WriteAllTextAsync(fileName, sb.ToString(), Encoding.UTF8);
                }

                Console.WriteLine($"Данные сохранены в файл: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }
    }
}
