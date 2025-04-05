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

        int mainChoice = 0;
        while (mainChoice != 5)
        {
            Console.WriteLine(@"Введіть дію:
            1. Показати список книжок
            2. Додати нову книжку
            3. Знайти книжку і провести дії з нею ( перегляд, додання к-сті, видалення, редагування ціни)
            5. Вийти з програми");
        
            mainChoice = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            
            Console.Clear();
            
            switch (mainChoice)
            {
                case 1:
                    foreach (Book book in books)
                    {
                        Console.WriteLine(book.ToString());
                    }
                    Console.WriteLine(@"Відсортувати за :
                    1.  За Автором
                    2.  За Назвою
                    3.  Показати найдешевші книжки
                    4.  Показати найдорожчі книжки ");
                    
                    int sortingChoice;
                    bool isValidSortingChoice = false;
                    do
                    {
                        var parsingResult = int.TryParse(Console.ReadLine(), out sortingChoice);
                        if (!parsingResult)
                        {
                            Console.WriteLine("Введене значення не є числом");
                            continue;
                        }
                        
                        if (sortingChoice is < 1 or > 3)
                        {
                            Console.WriteLine("Введене значення неправильне");
                            continue;
                        }
                        
                        isValidSortingChoice = true;
                    }
                    while (!isValidSortingChoice);
                    
                    Console.Clear();

                    List<Book> booksSorted = [];
                    string messageToConsole = string.Empty;
                    
                    switch (sortingChoice)
                    {
                        case 1:
                            booksSorted = BookSortService.SortByAuthor(books);
                            messageToConsole += "📚 Книги, відсортовані за автором:";
                            
                            break;
                        case 2:
                            booksSorted = BookSortService.SortByTitle(books);
                            messageToConsole += "📚 Книги, відсотовані за назвою:";
                            
                            break;
                        case 3:
                            booksSorted = BookSortService.SortByPriceAscending(books);
                            messageToConsole += "📚 Книги, відсортовані за зростанням ціни:";
                            
                            break;
                        case 4:
                            booksSorted = BookSortService.SortByPriceDescending(books);
                            messageToConsole += "\n📚 Книги, відсортовані за спаданням ціни:";
                            
                            break;
                    }
                    
                    Console.WriteLine(messageToConsole);
                    
                   
                    foreach (var book in booksSorted)
                    {
                        Console.WriteLine(book.ToString());
                    }
                    
                    
                    Console.WriteLine();
                    Console.WriteLine("Натисніть будь-яку клавішу щоб продовжити.");
                    Console.ReadKey();
                    Console.Clear();
                    
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
                    
                    Thread.Sleep(2000); 
                    
                    Console.Clear();
                    break;
                
                case 3:
                    
                    Console.WriteLine("Введіть пошукову інформацію");
                    string name = Console.ReadLine() ?? string.Empty;
                    if (name != null)
                    {
                        string pattern = $@"{Regex.Escape(name)}"; // Escaping input to avoid regex errors
                    
                        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

                        // Search through books
                        Console.WriteLine("Matching books:");
                        bool isFound = false;
                        
                        foreach (var book in books)
                        {
                            // Check if any title in the list matches the regex
                            if (book.Title.Any(title => regex.IsMatch(title)) ||
                                (book.Author.Any(author => regex.IsMatch(author))))
                            {
                                Console.WriteLine(book.ToString());
                                isFound = true;
                            }
                        }
                        if( isFound == false )
                        {
                            Console.WriteLine("Даної книжки немає у наявності");
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("Натисніть будь-яку клавішу щоб продовжити.");
                    Console.ReadKey();
                    Console.Clear();
                    
                    break;
            }
        }

    }
}

