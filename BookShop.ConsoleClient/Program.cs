namespace BookShop.ConsoleClient;

abstract class Program
{
    static void Main()
    {
        Book book1 = new Book("1984", "George Orwell", "Secker & Warburg", 19.99m);
        Console.WriteLine(book1);
        book1.Author = "George Orwell";
    }

}
