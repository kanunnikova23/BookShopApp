namespace BookShop.ConsoleClient;

using System;
using System.IO;

class Program
{
    static void Main()
    {
            string filePath = "/Users/oleksandrakanunnikova/RiderProjects/BookShopApp/BookShop.ConsoleClient/console_output";

            Console.WriteLine("Введіть текст для запису у файл:");
            string input = Console.ReadLine() ?? "";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(input);
            }

            Console.WriteLine($"Текст {input} записано у файл. Дані збережено у {filePath}" );

    }
}