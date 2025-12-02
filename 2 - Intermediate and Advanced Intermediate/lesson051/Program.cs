public class Program
{
    static void Main()
    {
        Console.WriteLine("=== Book Analyzer Demo ===\n");

        var books = new List<Book>
        {
            new Book { Title = "C# in Depth", Author = "Jon Skeet", Price = 50m, Genre = "Programming" },
            new Book { Title = "Clean Code", Author = "Robert C. Martin", Price = 45m, Genre = "Programming" },
            new Book { Title = "The Pragmatic Programmer", Author = "Andrew Hunt", Price = 55m, Genre = "Programming" },
            new Book { Title = "Harry Potter", Author = "J.K. Rowling", Price = 30m, Genre = "Fantasy" },
            new Book { Title = "Lord of the Rings", Author = "J.R.R. Tolkien", Price = 60m, Genre = "Fantasy" },
            new Book { Title = "Game of Thrones", Author = "George R.R. Martin", Price = 60m, Genre = "Fantasy" }
        };

        // 1️ Lấy sách theo thể loại
        string genreFilter = "Programming";
        Console.WriteLine($"[1] Books in Genre: {genreFilter}");
        var genreBooks = BookAnalyzer.GetBooksByGenre(books, genreFilter);
        foreach (var b in genreBooks)
        {
            Console.WriteLine($"{b.Title} by {b.Author} - ${b.Price}");
        }
        Console.WriteLine();

        // 2️ Tác giả có sách đắt nhất
        Console.WriteLine("[2] Author(s) with the Most Expensive Book:");
        var topAuthors = BookAnalyzer.GetAuthorsWithMostExpensiveBook(books);
        foreach (var author in topAuthors)
        {
            Console.WriteLine(author);
        }
        Console.WriteLine();

        // 3️ Giá trung bình theo thể loại
        Console.WriteLine("[3] Average Price by Genre:");
        var avgPrices = BookAnalyzer.GetAveragePriceByGenre(books);
        foreach (var kvp in avgPrices)
        {
            Console.WriteLine($"{kvp.Key}: ${kvp.Value:F2}");
        }

        Console.ReadKey();
    }
}

public class Book
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public decimal Price { get; set; }
    public required string Genre { get; set; }
}

public static class BookAnalyzer
{
    public static List<Book> GetBooksByGenre(List<Book> books, string genre)
    {
        return books
            .Where(b => b.Genre == genre)
            .OrderBy(b => b.Price)
            .ToList();
    }

    public static List<string> GetAuthorsWithMostExpensiveBook(List<Book> books)
    {
        decimal maxPrice = books.Max(b => b.Price);
        return books
            .Where(b => b.Price == maxPrice)
            .Select(b => b.Author)
            .Distinct()
            .ToList();
    }

    public static Dictionary<string, decimal> GetAveragePriceByGenre(List<Book> books)
    {
        return books
            .GroupBy(b => b.Genre)
            .ToDictionary(g => g.Key, g => g.Average(b => b.Price));
    }
}

/*
 === Book Analyzer Demo ===

[1] Books in Genre: Programming
Clean Code by Robert C. Martin - $45
C# in Depth by Jon Skeet - $50
The Pragmatic Programmer by Andrew Hunt - $55

[2] Author(s) with the Most Expensive Book:
J.R.R. Tolkien
George R.R. Martin

[3] Average Price by Genre:
Programming: $50.00
Fantasy: $50.00
 */

/*
* 1.GetBooksByGenre:

Filters books by genre, sorts them by price, and returns the result.

* 2.GetAuthorsWithMostExpensiveBook:

Finds the maximum price, then filters and selects unique authors with books at that price.

* 3.GetAveragePriceByGenre:

Groups books by genre, calculates the average price in each group, and returns it as a dictionary.
 
 */