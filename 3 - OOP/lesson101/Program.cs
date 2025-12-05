public class Program
{
    static void Main()
    {
        Console.WriteLine("===== LIBRARY BOOK FILTER DEMO =====");
        Console.WriteLine();

        var library = new Library();

        library.AddBook(new Book("1984", "George Orwell", 1949, "Dystopian", 9.2));
        library.AddBook(new Book("Animal Farm", "George Orwell", 1945, "Political Satire", 8.7));
        library.AddBook(new Book("Dune", "Frank Herbert", 1965, "Science Fiction", 9.5));
        library.AddBook(new Book("Foundation", "Isaac Asimov", 1951, "Science Fiction", 9.0));
        library.AddBook(new Book("The Hobbit", "J.R.R. Tolkien", 1937, "Fantasy", 9.1));

        // --- FILTER DEMOS ---
        Console.WriteLine("== Books in genre: Science Fiction ==");
        var sciFiBooks = library.FilterByGenre("Science Fiction");
        PrintBooks(sciFiBooks);

        Console.WriteLine("\n== Books by George Orwell ==");
        var orwellBooks = library.FilterByAuthor("George Orwell");
        PrintBooks(orwellBooks);

        Console.WriteLine("\n== Books published in 1945 ==");
        var yearBooks = library.FilterByYear(1945);
        PrintBooks(yearBooks);

        Console.WriteLine("\n== Top 3 Rated Books ==");
        var topBooks = library.GetTopRatedBooks(3);
        PrintBooks(topBooks);

        Console.ReadKey();
    }

    static void PrintBooks(List<Book> books)
    {
        if (books.Count == 0)
        {
            Console.WriteLine("No books found.");
            return;
        }

        foreach (var b in books)
        {
            Console.WriteLine($"- {b.Title} ({b.Author}), {b.YearPublished} | Genre: {b.Genre} | Rating: {b.Rating}");
        }
    }
}

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int YearPublished { get; set; }
    public string Genre { get; set; }
    public double Rating { get; set; }

    public Book(string title, string author, int yearPublished, string genre, double rating)
    {
        Title = title;
        Author = author;
        YearPublished = yearPublished;
        Genre = genre;
        Rating = rating;
    }
}

public class Library
{
    private readonly List<Book> _books = new List<Book>();

    public void AddBook(Book book)
    {
        _books.Add(book);
    }

    public List<Book> FilterByGenre(string genre)
    {
        return _books.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Book> FilterByAuthor(string author)
    {
        return _books.Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Book> FilterByYear(int year)
    {
        return _books.Where(b => b.YearPublished == year).ToList();
    }

    public List<Book> GetTopRatedBooks(int count)
    {
        return _books.OrderByDescending(b => b.Rating).Take(count).ToList();
    }
}

/*
* 1.Book Class:

    Holds properties for book details such as title, author, year, genre, and rating.

* 2.Library Class with Encapsulated Book List:

    ddBook allows adding books to the library’s private _books list.

    FilterByGenre, FilterByAuthor, and FilterByYear use LINQ Where to filter books.

    GetTopRatedBooks sorts books by rating in descending order and takes the specified count.
 */