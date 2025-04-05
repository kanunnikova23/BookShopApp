namespace BookShop.ConsoleClient;

public static class BookSortService
{
    
    public static List<Book> SortByAuthor(List<Book> list)
    {
        return list.OrderBy(book => book.Author).ToList();
    }

    public static List<Book> SortByTitle(List<Book> books1)
    {
        return books1.OrderBy(book => book.Title).ToList();
    }

    public static List<Book> SortByPriceAscending(List<Book> list1)
    {
        return list1.OrderBy(book => book.Price).ToList();
    }

    public static List<Book> SortByPriceDescending(List<Book> books2)
    {
        return books2.OrderByDescending(book => book.Price).ToList();
    }
}