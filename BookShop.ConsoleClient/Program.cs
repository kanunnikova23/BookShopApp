using Newtonsoft.Json;

namespace BookShop.ConsoleClient;

using System;
using System.IO;

class Program
{
    static async Task Main()
    {
        string filePath = "/Users/oleksandrakanunnikova/RiderProjects/BookShopApp/BookShop.ConsoleClient/books.json";

        List<Book> books = new List<Book>();

        if (File.Exists(filePath))
        {
            string jsonContent = await File.ReadAllTextAsync(filePath);
            if (!string.IsNullOrWhiteSpace(jsonContent))
            {
                try
                {
                    books = JsonConvert.DeserializeObject<List<Book>>(jsonContent) ?? new List<Book>();
                }
                catch (JsonSerializationException)
                {
                    Console.WriteLine("❌ При читанні файлу виникла помилка серіалізації.");
                }
            }
        }
        
        
        Console.WriteLine("Введіть кількість книжок до додання:");
        int numberOfBooks = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

        for (int i = 0; i < numberOfBooks; i++)
        {
            Console.WriteLine("Для створення книжки вам потрібно ввести інформацію стосовно неї.");

            List<string> titles = new List<string>();
            
            Console.Write("Введіть назву книжки: ");
            titles.Add(Console.ReadLine()?.Trim() ?? "Невідома назва книги");

            while (true)
            {
                Console.Write("Чи є у книжки ще одна альтернативна назва? (введіть так або ні): ");
                string answer = Console.ReadLine()?.Trim().ToLower() ?? "";
                if (answer == "так")
                {
                    Console.Write("Введіть альтернативну назву книжки: ");
                    titles.Add(Console.ReadLine()?.Trim() ?? "Невідома назва книги");
                }
                else if (answer == "ні")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Невірне значення, спробуйте ще раз.");
                }
            }

            List<string> authors = new List<string>();
            
            Console.Write("Введіть автора книжки: ");
            authors.Add(Console.ReadLine()?.Trim() ?? "Невідомий автор");

            while (true)
            {
                Console.Write("Чи є у книжки ще один автор? (введіть так або ні): ");
                string answer = Console.ReadLine()?.Trim().ToLower() ?? "";
                if (answer == "так")
                {
                    Console.Write("Введіть ім'я автора: ");
                    authors.Add(Console.ReadLine()?.Trim() ?? "Невідомий автор");
                }
                else if (answer == "ні")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("❌ Невірне значення, спробуйте ще раз.");
                }
            }

            decimal price;
            while (true)
            {
                Console.Write("Введіть ціну книжки у грн: ");
                if (decimal.TryParse(Console.ReadLine(), out price) && price >= 0)
                    break;
                Console.WriteLine("❌ Помилка: введіть коректну ціну (невід’ємне число).");
            }

            Book book = new Book(titles, authors, price);
            books.Add(book);
            Console.WriteLine("✅ Додано книгу: " + book);
        }

        await File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(books, Formatting.Indented));
        Console.WriteLine("✅ Всі книги успішно збережено у файлі.");
        }
}

