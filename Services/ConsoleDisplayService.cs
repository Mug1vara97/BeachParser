using System;

namespace BeachParser.Services
{
    public static class ConsoleDisplayService
    {
        public static void ShowWelcome()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("ПАРСЕР ИНФОРМАЦИИ О ПЛЯЖАХ");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Используемые API:");
            Console.WriteLine("   • OpenWeatherMap - данные о погоде");
            Console.WriteLine("   • OpenStreetMap - геолокация пляжей");
            Console.WriteLine();
            Console.WriteLine("Примеры пляжей для поиска:");
            Console.WriteLine("   • Copacabana Beach");
            Console.WriteLine("   • Bondi Beach");
            Console.WriteLine("   • Miami Beach");
            Console.WriteLine("   • Nice Beach");
            Console.WriteLine("   • Barcelona Beach");
            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════════════");
        }
    }
}
