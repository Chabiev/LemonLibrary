using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Application.Services;
using Database.Data;
using Business.DTOs;
using Database.Entities;

public class BooksService : IBooksService
{
    private readonly LibraryContext _context;

    public BooksService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<BookDTO>> GetBooks()
    {

        var books = await _context.Books
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .ToListAsync();
        if (books.Count == 0)
        {
            throw new InvalidOperationException("No book was found");
        }

        var bookDTOs = books.Select(book => new BookDTO
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Rating = book.Rating,
            DateAdded = book.DateAdded,
            Available = book.available,

            Authors = book.BookAuthors.Select(ba => new AuthorDTO
            {
                Id = ba.Author.Id,
                FirstName = ba.Author.FirstName,
                LastName = ba.Author.LastName,
                BirthDate = ba.Author.BirthDate,
            }).ToList()
        }).ToList();



        return bookDTOs;
    }

    public async Task<BookDTO> GetBookById(int id)
    {
        var book = await _context.Books
            .Where(b => b.Id == id)
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .SingleOrDefaultAsync();

        if (book == null)
        {
            throw
                new ArgumentNullException("No book Id"); 
        }

        var bookDTO = new BookDTO
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Rating = book.Rating,
            DateAdded = book.DateAdded,
            Available = book.available,

            Authors = book.BookAuthors.Select(ba => new AuthorDTO
            {
                Id = ba.Author.Id,
                FirstName = ba.Author.FirstName,
                LastName = ba.Author.LastName,
                BirthDate = ba.Author.BirthDate,
            }).ToList()
        };

        return bookDTO; 
    }

    public async Task AddBookWithAuthor(BookAuthorDTO bookAuthorDTO)
    {
        Author existingAuthor = await _context.Authors.FirstOrDefaultAsync(a =>
            a.FirstName.ToLower() == bookAuthorDTO.FirstName.ToLower() &&
            a.LastName.ToLower() == bookAuthorDTO.LastName.ToLower());

        bool titleExists = await _context.Books.AnyAsync(b =>
            b.Title.ToLower() == bookAuthorDTO.Title.ToLower());

        if (titleExists)
        {
            throw new ArgumentException("A book with the same title already exists.");
        }

        if (existingAuthor == null)
        {
            existingAuthor = new Author
            {
                FirstName = bookAuthorDTO.FirstName,
                LastName = bookAuthorDTO.LastName,
                BirthDate = bookAuthorDTO.BirthDate
            };
        }

        var book = new Book
        {
            Title = bookAuthorDTO.Title,
            Description = bookAuthorDTO.Description,
            Rating = bookAuthorDTO.Rating,
            DateAdded = bookAuthorDTO.DateAdded,
            available = bookAuthorDTO.Available
        };

        book.BookAuthors = new[]
        {
            new BookAuthor { Author = existingAuthor }
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
    }



    public async Task EditBook(EditBookDTO editBookDTO)
    {
        var book = await _context.Books
            .Where(b => b.Id == editBookDTO.BookId)
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .SingleOrDefaultAsync();

        if (book == null)
        {
            throw new ArgumentException("No book with the given ID was found.");
        }

        if (editBookDTO.AuthorId.HasValue)
        {
            var existingAuthor = await _context.Authors.FindAsync(editBookDTO.AuthorId.Value);

            if (existingAuthor == null)
            {
                throw new ArgumentException("The specified author does not exist.");
            }

            var existingBookAuthor = book.BookAuthors.FirstOrDefault();
            if (existingBookAuthor != null)
            {
                existingBookAuthor.Author = existingAuthor;
            }
            else
            {
                book.BookAuthors.Add(new BookAuthor { Author = existingAuthor });
            }
        }
        else 
        {
            var newAuthor = new Author
            {
                FirstName = editBookDTO.FirstName,
                LastName = editBookDTO.LastName,
                BirthDate = editBookDTO.BirthDate
            };

            var existingBookAuthor = book.BookAuthors.FirstOrDefault();
            if (existingBookAuthor != null)
            {
                existingBookAuthor.Author = newAuthor;
            }
            else
            {
                book.BookAuthors.Add(new BookAuthor { Author = newAuthor });
            }
        }

        book.Title = editBookDTO.Title;
        book.Description = editBookDTO.Description;
        book.Rating = editBookDTO.Rating;

        await _context.SaveChangesAsync();

    }

    public async Task ToggleBookStatus(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);

        if (book == null)
        {
            throw new ArgumentException("No book with the given ID was found.");
        }

        book.available = !book.available;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteBookById(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);

        if (book == null)
        {
            throw new ArgumentException("No book with the given ID was found.");
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

    }
}