namespace BookShop.ConsoleClient;

public class Book
{
    public Book(string title, string author, decimal price)
    {
        this.Title = title;
        this.Author = author;
        this.Price = price;
    }
    public Book(): this(string.Empty,string.Empty,0){}
    public string Title { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }


    public override string ToString()
    {
        return $"Title: {this.Title}, Author: {this.Author}, Price: {this.Price}";
    }
}
