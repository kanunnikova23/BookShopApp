namespace BookShop.ConsoleClient.Models;

public class Book(List<string?> title, List<string> author, decimal price)
{
    private int _publicationYear;
    public Book(): this([],[],0){}
    public List<string?> Title { get; set; } = title;
    public List<string> Author { get; set; } = author;
    public decimal Price { get; set; } = price;
    
    // Кількість доступних екземплярів (за замовчуванням 0)
    public int Quantity { get; set; } = 0;
    
    // Міжнародний стандартний книжковий номер
    public string ISBN { get; set; }

    // Рік публікації (наприклад, 2021)
    public int PublicationYear
    {
        get => _publicationYear;
        set
        {
            if (value < 1450 || value > DateTime.Now.Year)
            {
                throw new ArgumentOutOfRangeException(nameof(PublicationYear),
                    "Year must be between 1450 and current year.");
            }

            _publicationYear = value;
        }
    }

    //жанр книжки
    public Genre Genre { get; set; }


    public override string ToString()
    {
        return $"Назва: '{string.Join(", ", this.Title)}', Автор: {string.Join(", ", this.Author)}, Ціна: {this.Price} грн.";
    }

}
