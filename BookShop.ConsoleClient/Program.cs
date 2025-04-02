using System.Text.RegularExpressions;
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

        int choice = 0;
        while (choice != 5)
        {
            Console.WriteLine(@"Введіть дію:
            1. Показати список книжок
            2. Додати книжку
            3. Видалити книжку
            4. Знайти книжку
            5. Вийти з програми");
        
        choice = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());;
        switch (choice)
        {
            case 1:
                foreach (Book book in books)
                {
                    Console.WriteLine(book.ToString());
                }
                break;
            
            case 2:
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
                break;
            
            case 3:
                break;
            case 4:
                // Define regex pattern (example: searching for books with "C#" in the title)
                Console.WriteLine("Введіть пошукову інформацію");
                string name = Console.ReadLine() ?? string.Empty;
                if (name != null)
                {
                    string pattern = $@"{Regex.Escape(name)}"; // Escaping input to avoid regex errors
                
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

                    // Search through books
                    Console.WriteLine("Matching books:");
                    foreach (var book in books)
                    {
                        // Check if any title in the list matches the regex
                        if (book.Title.Any(title => regex.IsMatch(title)) || (book.Author.Any(author => regex.IsMatch(author))))
                        {
                            Console.WriteLine("Книжка була знайдена");
                            Console.WriteLine(book.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Даної книжки немає у наявності");
                        }
                    }
                }

                break;
        }
        }
       
        
        
    }
}

