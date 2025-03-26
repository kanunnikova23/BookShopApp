namespace BookShop.ConsoleClient;

public class Book(List<string> title, List<string> author, decimal price)
{
    public Book(): this([],[],0){}
    public List<string> Title { get; set; } = title;
    public List<string> Author { get; set; } = author;
    public decimal Price { get; set; } = price;


    public override string ToString()
    {
        return $"Назва: {string.Join(", ", this.Title)}, Автор: {string.Join(", ", this.Author)}, Ціна: {this.Price}";
    }

}
