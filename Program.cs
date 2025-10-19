using System;
using System.Threading.Tasks;
using BeachParser.DTO;
using BeachParser.Services;

namespace BeachParser
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            const string WEATHER_API_KEY = "c3c4366e6cfa32405c6b4b22a21a6dbf";
            
            var parser = new BeachParserService(WEATHER_API_KEY);
            
            try
            {
                ConsoleDisplayService.ShowWelcome();
                
                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Введите название пляжа (или 'exit' для выхода):");
                    Console.Write("> ");
                    
                    var input = Console.ReadLine()?.Trim();
                    
                    if (string.IsNullOrEmpty(input))
                    {
                    Console.WriteLine("Пожалуйста, введите название пляжа");
                    continue;
                }
                
                if (input.ToLower() == "exit" || input.ToLower() == "выход")
                {
                    Console.WriteLine("До свидания!");
                    break;
                }
                    
                    var beachData = await parser.SearchBeachAsync(input);
                    
                    if (beachData == null)
                    {
                        Console.WriteLine("Пляж не найден. Попробуйте другое название.");
                        continue;
                    }
                    
                    parser.DisplayBeachData(beachData);
                    
                    await OfferSaveOptions(parser, beachData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
            }
            finally
            {
                parser.Dispose();
            }
        }
        
        
        static async Task OfferSaveOptions(BeachParserService parser, BeachData beachData)
        {
            Console.WriteLine();
            Console.WriteLine("Хотите сохранить данные?");
            Console.WriteLine("   1. Сохранить в JSON");
            Console.WriteLine("   2. Сохранить в TXT");
            Console.WriteLine("   3. Продолжить без сохранения");
            Console.Write("Выберите опцию (1-3): ");
            
            var choice = Console.ReadLine()?.Trim();
            
            switch (choice)
            {
                case "1":
                    await parser.SaveToFileAsync(beachData, "json");
                    break;
                case "2":
                    await parser.SaveToFileAsync(beachData, "txt");
                    break;
                case "3":
                    Console.WriteLine("Данные не сохранены");
                    break;
                default:
                    Console.WriteLine("Неверный выбор, данные не сохранены");
                    break;
            }
        }
    }
}