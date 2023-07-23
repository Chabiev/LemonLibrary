using Application.Interfaces;
using Business.DTOs;
using Database.Data;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AuthorService : IAuthorService
{
    private readonly LibraryContext _context;

    public AuthorService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<AuthorDTO>> GetAuthors()
    {
        var books = await _context.Books
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .ToListAsync();

        var authorBooksMap = new Dictionary<Author, List<Book>>();

        foreach (var book in books)
        {
            foreach (var bookAuthor in book.BookAuthors)
            {
                if (!authorBooksMap.ContainsKey(bookAuthor.Author))
                {
                    authorBooksMap[bookAuthor.Author] = new List<Book>();
                }

                authorBooksMap[bookAuthor.Author].Add(book);
            }
        }

        var authorDTOs = authorBooksMap.Select(kv => new AuthorDTO
        {
            Id = kv.Key.Id,
            FirstName = kv.Key.FirstName,
            LastName = kv.Key.LastName,
            BirthDate = kv.Key.BirthDate,

            Books = kv.Value.Select(book => new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Rating = book.Rating,
                DateAdded = book.DateAdded,
                Available = book.available,
            }).ToList()
        }).ToList();

        if (authorDTOs.Count == 0)
        {
            throw new InvalidOperationException();
        }

        return authorDTOs;
    }


    public async Task<AuthorDTO> GetAuthorById(int id)
    {
        var author = await _context.Authors
            .Where(a => a.Id == id)
            .Include(a => a.BookAuthors)
            .ThenInclude(ba => ba.Book)
            .SingleOrDefaultAsync();

        if (author == null)
        {
            throw new ArgumentException("No author with given ID was found");
        }

        var booksDTO = author.BookAuthors.Select(ba => new BookDTO
        {
            Id = ba.Book.Id,
            Title = ba.Book.Title,
            Description = ba.Book.Description,
            Rating = ba.Book.Rating,
            DateAdded = ba.Book.DateAdded,
            Available = ba.Book.available,
        }).ToList();

        var authorDTO = new AuthorDTO
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            BirthDate = author.BirthDate,

            Books = booksDTO
        };

        return authorDTO;
    }

    public async Task AddAuthor(AddAuthorDTO addAuthorDto)
    {
        string normalizedFirstName = addAuthorDto.FirstName.ToLower();
        string normalizedLastName = addAuthorDto.LastName.ToLower();

        var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a =>
            a.FirstName.ToLower() == normalizedFirstName &&
            a.LastName.ToLower() == normalizedLastName);

        if (existingAuthor != null)
        {
            throw new ArgumentException("This author is already added");
        }

        var newAuthor = new Author
        {
            FirstName = addAuthorDto.FirstName,
            LastName = addAuthorDto.LastName,
            BirthDate = addAuthorDto.BirthDate
        };

        _context.Authors.Add(newAuthor);
        await _context.SaveChangesAsync();

    }

    public async Task EditAuthor(EditAuthorDTO editAuthorDTO)
    {
        // Fetch the specific author with the given ID from the database
        var author = await _context.Authors
            .Where(b => b.Id == editAuthorDTO.AuthorId)
            .SingleOrDefaultAsync();

        if (author == null)
        {
            throw new ArgumentException("No author with the given ID was found.");
        }

        author.FirstName = editAuthorDTO.FirstName;
        author.LastName = editAuthorDTO.LastName;
        author.BirthDate = editAuthorDTO.BirthDate;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAuthorById(int authorId)
    {
        var author = await _context.Authors.FindAsync(authorId);

        if (author == null)
        {
            throw new ArgumentException("No author with the given ID was found.");
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }
}