using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace BookShop.ConsoleClient;

internal static class Program
{
    private static async Task Main()
    {
        var filePath = "/Users/oleksandrakanunnikova/RiderProjects/BookShopApp/BookShop.ConsoleClient/books.json";

        var books = new List<Book>();

        if (File.Exists(filePath))
        {
            var jsonContent = await File.ReadAllTextAsync(filePath);
            if (!string.IsNullOrWhiteSpace(jsonContent))
                try
                {
                    books = JsonConvert.DeserializeObject<List<Book>>(jsonContent) ?? new List<Book>();
                }
                catch (JsonSerializationException)
                {
                    Console.WriteLine("❌ При читанні файлу виникла помилка серіалізації.");
                }
        }


        var menuChoice = 0;
        var isValidMenuChoice = false;

        while (menuChoice != 4)
        {
            Console.WriteLine(@"Введіть дію:
            1. Показати список книжок
            2. Додати нову книжку
            3. Знайти книжку і провести дії з нею ( перегляд, додання к-сті, видалення, редагування ціни)
            4. Вийти з програми");

            do
            {
                var parsingResult = int.TryParse(Console.ReadLine(), out menuChoice);
                if (!parsingResult)
                {
                    Console.WriteLine("Введене значення не є числом");
                    continue;
                }

                if (menuChoice is < 1 or > 4)
                {
                    Console.WriteLine("Введене значення неправильне");
                    continue;
                }

                if (menuChoice == 4) break;

                isValidMenuChoice = true;
            } while (!isValidMenuChoice);


            switch (menuChoice)
            {
                case 1:
                    foreach (var book in books) Console.WriteLine(book.ToString());
                    Console.WriteLine(@"Відсортувати за :
                    1.  За Автором
                    2.  За Назвою
                    3.  Показати найдешевші книжки
                    4.  Показати найдорожчі книжки ");

                    int sortingChoice;
                    var isValidSortingChoice = false;
                    do
                    {
                        var parsingResult = int.TryParse(Console.ReadLine(), out sortingChoice);
                        if (!parsingResult)
                        {
                            Console.WriteLine("Введене значення не є числом");
                            continue;
                        }

                        if (sortingChoice is < 0 or > 5)
                        {
                            Console.WriteLine("Введене значення неправильне");
                            continue;
                        }

                        isValidSortingChoice = true;
                    } while (!isValidSortingChoice);

                    Console.Clear();

                    List<Book> booksSorted = [];
                    var messageToConsole = string.Empty;

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

                    foreach (var book in booksSorted) Console.WriteLine(book.ToString());

                    PressKeyToContinue();

                    break;

                case 2:
                    Console.WriteLine("Введіть кількість книжок до додання (Max 10) :");
                    int numberOfBooks;

                    var isValidEntry = false;
                    do
                    {
                        var parsingResult = int.TryParse(Console.ReadLine(), out numberOfBooks);

                        if (!parsingResult)
                        {
                            Console.WriteLine("Введене значення не є числом");
                            continue;
                        }

                        if (numberOfBooks is < 1 or > 10)
                        {
                            Console.WriteLine("Введене значення неправильне");
                            continue;
                        }

                        isValidEntry = true;
                    } while (!isValidEntry);


                    for (var i = 0; i < numberOfBooks; i++)
                    {
                        Console.WriteLine("Для створення книжки вам потрібно ввести інформацію стосовно неї.");

                        var titles = new List<string?>();

                        var hasMoreTitles = true;

                        while (hasMoreTitles)
                        {
                            // цикл на правильне введення назви
                            string title;
                            while (true)
                            {
                                Console.Write("Введіть назву книжки: ");
                                var isBookTitleValid = EnterBookTitle(out title);

                                if (isBookTitleValid == 0) break; // вихід з цього циклу, назва валідна

                                switch (isBookTitleValid)
                                {
                                    case 1:
                                        Console.WriteLine("❌  Введіть назву книги коректно.");
                                        break;
                                    case 2:
                                        Console.WriteLine("❌  Помилка трапилася...Зненацька...");
                                        break;
                                }
                            }

                            titles.Add(title);

                            // цикл на правильну відповідь "так" або "ні"
                            while (true)
                            {
                                Console.Write("Чи є у книжки ще одна альтернативна назва? (введіть так або ні): ");
                                if (GetAnswer(ref hasMoreTitles)) break;
                            }
                        }


                        var authors = new List<string>();

                        var hasMoreAuthors = true;

                        while (hasMoreAuthors)
                        {
                            Console.Write("Введіть автора книжки: ");

                            authors.Add(Console.ReadLine()?.Trim() ?? "Невідомий автор");


                            // цикл на правильну відповідь "так" або "ні"
                            while (true)
                            {
                                Console.Write("Чи є у книжки ще один автор? (введіть так або ні): ");
                                if (GetAnswer(ref hasMoreAuthors)) break;
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

                        var book = new Book(titles, authors, price);
                        books.Add(book);
                        Console.WriteLine("✅ Додано книгу: " + book);
                    }

                    await File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(books, Formatting.Indented));
                    Console.WriteLine("✅ Всі книги успішно збережено у файлі.");

                    Thread.Sleep(3000);

                    PressKeyToContinue();
                    break;

                case 3:

                    Console.WriteLine("Введіть пошукову інформацію");
                    var name = Console.ReadLine();
                    // : IMPLEMENT NULL VALUES HANDLING
                    if (name != string.Empty)
                    {
                        var pattern = $@"{Regex.Escape(name)}"; // Escaping input to avoid regex errors

                        var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                        // Search through books
                        Console.WriteLine("Matching books:");
                        var isFound = false;

                        foreach (var book in books)
                            // Check if any title in the list matches the regex
                            if (book.Title.Any(title => regex.IsMatch(title)) ||
                                book.Author.Any(author => regex.IsMatch(author)))
                            {
                                Console.WriteLine(book.ToString());
                                isFound = true;
                            }

                        if (isFound == false) Console.WriteLine("Даної книжки немає у наявності");
                    }

                    PressKeyToContinue();

                    break;
            }
        }

        void PressKeyToContinue()
        {
            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу щоб продовжити.");
            Console.ReadKey();
            Console.Clear();
        }

        bool GetAnswer(ref bool hasMoreNames)
        {
            var answer = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (answer == "так") return true;

            if (answer == "ні")
            {
                hasMoreNames = false;
                return true;
            }

            Console.WriteLine("❌ Невірне значення, спробуйте ще раз.");
            return false;
        }
    }


    private static int EnterBookTitle(out string title)
    {
        title = Console.ReadLine() ?? string.Empty;

        if (title == string.Empty) return 1;
        title = title.Trim();

        return 0;
    }
}