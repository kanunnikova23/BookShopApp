using Newtonsoft.Json;

namespace BookShop.ConsoleClient;

using System;
using System.IO;

class Program
{
    static void Main()
    {
            string filePath = "/Users/oleksandrakanunnikova/RiderProjects/BookShopApp/BookShop.ConsoleClient/books.json";
            
            List<Book>? books;
        
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                books = JsonConvert.DeserializeObject<List<Book>>(fileContent);
            }
            else
            {
                books = new List<Book>();
            }
            
            Console.WriteLine("Введіть кількість книжок до додання");
            int numberOfBooks = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            
            for (int i = 0; i < numberOfBooks; i++)
            {
                // Відкриваємо файл для додавання нових книг
                using (new StreamWriter(filePath, append: true))
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

                    Book book = new Book(title, author, price);

                    books?.Add(book);

                    Console.WriteLine("\n✅ Книжку додано до файлу:");
                    Console.WriteLine(book);
                }

                File.WriteAllText(filePath, JsonConvert.SerializeObject(books, Formatting.Indented));
            }
    }
    
}