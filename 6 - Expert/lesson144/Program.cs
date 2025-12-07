// ======================================================================
// PROGRAM ENTRY – UNIT OF WORK DEMO
// ======================================================================
public class Program
{
    static void Main()
    {
        Console.WriteLine("=== UNIT OF WORK + REPOSITORY DEMO ===\n");

        // Khởi tạo repository cho Book & Author
        var bookRepo = new BookRepository();
        var authorRepo = new AuthorRepository();

        // Tạo UnitOfWork
        using (var uow = new UnitOfWork(bookRepo, authorRepo))
        {
            // Tạo một Author
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "J.K. Rowling"
            };

            uow.Authors.Add(author);

            // Tạo một Book
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Harry Potter and the Philosopher's Stone",
                AuthorId = author.Id
            };

            uow.Books.Add(book);

            Console.WriteLine("Book + Author added but not committed yet.");

            // Commit toàn bộ thay đổi
            uow.Commit();

            // Truy vấn lại dữ liệu
            Console.WriteLine("\n>>> Querying book...");
            var fetchedBook = uow.Books.Get(book.Id);
            Console.WriteLine($"Book: {fetchedBook.Title}");

            Console.WriteLine("\n>>> Querying author...");
            var fetchedAuthor = uow.Authors.Get(author.Id);
            Console.WriteLine($"Author: {fetchedAuthor.Name}");

            // Remove
            Console.WriteLine("\n>>> Removing book...");
            uow.Books.Remove(book);
            uow.Commit();
        }

        Console.WriteLine("\n=== DONE ===");
        Console.ReadKey();
    }
}


// ======================================================================
// DOMAIN ENTITIES
// ======================================================================
public class Book
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public Guid AuthorId { get; set; }
}

public class Author
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}


// ======================================================================
// REPOSITORY INTERFACES
// ======================================================================
public interface IBookRepository
{
    void Add(Book book);
    Book Get(Guid id);
    void Remove(Book book);
}

public interface IAuthorRepository
{
    void Add(Author author);
    Author Get(Guid id);
    void Remove(Author author);
}


// ======================================================================
// UNIT OF WORK INTERFACE
// ======================================================================
public interface IUnitOfWork : IDisposable
{
    IBookRepository Books { get; }
    IAuthorRepository Authors { get; }
    void Commit();
}


// ======================================================================
// REPOSITORY IMPLEMENTATIONS
// ======================================================================
public class BookRepository : IBookRepository
{
    private readonly List<Book> _books = new();

    public void Add(Book book) => _books.Add(book);
    public Book Get(Guid id) => _books.Find(b => b.Id == id);
    public void Remove(Book book) => _books.Remove(book);
}

public class AuthorRepository : IAuthorRepository
{
    private readonly List<Author> _authors = new();

    public void Add(Author author) => _authors.Add(author);
    public Author Get(Guid id) => _authors.Find(a => a.Id == id);
    public void Remove(Author author) => _authors.Remove(author);
}


// ======================================================================
// UNIT OF WORK IMPLEMENTATION
// ======================================================================
public class UnitOfWork : IUnitOfWork
{
    public IBookRepository Books { get; }
    public IAuthorRepository Authors { get; }

    private bool _isDisposed;

    public UnitOfWork(IBookRepository bookRepository, IAuthorRepository authorRepository)
    {
        Books = bookRepository;
        Authors = authorRepository;
    }

    public void Commit()
    {
        Console.WriteLine("Committing changes to database...");
        // Ở ứng dụng thực tế, Commit() sẽ commit transaction DB
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            Console.WriteLine("Disposing Unit of Work resources...");
            _isDisposed = true;
        }
    }
}

/*
 === UNIT OF WORK + REPOSITORY DEMO ===

Book + Author added but not committed yet.
Committing changes to database...

>>> Querying book...
Book: Harry Potter and the Philosopher's Stone

>>> Querying author...
Author: J.K. Rowling

>>> Removing book...
Committing changes to database...
Disposing Unit of Work resources...

=== DONE ===

 */

/*
* 1.Entities (Book and Author): Represent the entities to be managed in the library system.

* 2.Repositories: BookRepository and AuthorRepository handle data access operations for Book and Author entities, respectively.

* 3.UnitOfWork: Manages both repositories and coordinates data operations across them. The Commit method simulates saving all changes in a single transaction.
 */