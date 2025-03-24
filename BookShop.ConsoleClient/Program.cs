using System.Text.Json;
using Newtonsoft.Json;

namespace BookShop.ConsoleClient;

using System;
using System.IO;

class Program
{
    static void Main()
    {
            // string filePath = "/Users/oleksandrakanunnikova/RiderProjects/BookShopApp/BookShop.ConsoleClient/console_output";

            // Console.WriteLine("Введіть текст для запису у файл:");
            // string input = Console.ReadLine() ?? "";
            //
            // using (StreamWriter writer = new StreamWriter(filePath, true))
            // {
            //     writer.WriteLine(input);
            // }
            //
            // Console.WriteLine($"Текст {input} записано у файл. Дані збережено у {filePath}" );
            
            
            
            // List<Book> books = new List<Book>
            // {
            //     new Book("1984", "George Orwell", 19.99m),
            //     new Book("Brave New World", "Aldous Huxley", 15.50m),
            //     new Book("The Great Gatsby", "F. Scott Fitzgerald", 10.99m),
            //     new Book("To Kill a Mockingbird", "Harper Lee", 12.75m),
            //     new Book("Moby-Dick", "Herman Melville", 22.30m)
            // };


            string filePath = "/Users/oleksandrakanunnikova/RiderProjects/BookShopApp/BookShop.ConsoleClient/books.json";

            // Відкриваємо файл для додавання нових книг
            using (StreamWriter writer = new StreamWriter(filePath, append: true))
            {
                Console.WriteLine("Для створення книжки вам потрібно ввести інформацію стосовно неї.");

                Console.Write("Введіть назву книжки: ");
                string title = Console.ReadLine() ?? "Невідома книга";

                Console.Write("Введіть автора книжки: ");
                string author = Console.ReadLine() ?? "Невідомий автор";

                decimal price;
                while (true)
                {
                    Console.Write("Введіть ціну книжки: ");
                    if (decimal.TryParse(Console.ReadLine(), out price) && price >= 0)
                        break;

                    Console.WriteLine("❌ Помилка: введіть коректну ціну (невід’ємне число).");
                }

                Book book1 = new Book(title, author, price);

                // Записуємо книгу у форматі JSON на окремому рядку
                string json = JsonConvert.SerializeObject(book1);
                writer.WriteLine(json);  // Записуємо нову книгу в файл в JSON форматі

                Console.WriteLine("\n✅ Книжку додано до файлу:");
                Console.WriteLine(book1);
            }
    }
    
}