using Lemon_Library.Data;
using Lemon_Library.DTOs;
using Lemon_Library.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lemon_Library.Controllers;

public class AuthorsController : BaseApiController
{
    private readonly LibraryContext _context;

    public AuthorsController(LibraryContext context)
    {
        _context = context;
    }
    //TODO: ავტორების სრული სიის წამოღება
    
    // GET: api/authors
    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        try
        {
            var books = await _context.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .ToListAsync();

            // Create a dictionary to store books grouped by their authors
            var authorBooksMap = new Dictionary<Author, List<Book>>();

            foreach (var book in books)
            {
                foreach (var bookAuthor in book.BookAuthors)
                {
                    // Check if the author already exists in the dictionary
                    if (!authorBooksMap.ContainsKey(bookAuthor.Author))
                    {
                        authorBooksMap[bookAuthor.Author] = new List<Book>();
                    }

                    // Add the current book to the author's list of books
                    authorBooksMap[bookAuthor.Author].Add(book);
                }
            }

            // Create a list of AuthorDTO objects with their corresponding books
            var authorDTOs = authorBooksMap.Select(kv => new AuthorDTO
            {
                Id = kv.Key.Id,
                FirstName = kv.Key.FirstName,
                LastName = kv.Key.LastName,
                BirthDate = kv.Key.BirthDate,
                // Optionally, you can include other author properties if needed

                // Map the books of each author to BookDTO objects
                Books = kv.Value.Select(book => new BookDTO
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Rating = book.Rating,
                    DateAdded = book.DateAdded,
                    Available = book.available,
                    // Optionally, you can include other book properties if needed
                }).ToList()
            }).ToList();

            return Ok(authorDTOs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    
    //TODO: დეტალების წამოღება
    
    // GET: api/authors/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthor(int id)
    {
        try
        {
            // Fetch the specific author with the chosen ID from the database
            var author = await _context.Authors
                .Where(a => a.Id == id)
                .Include(a => a.BookAuthors)
                .ThenInclude(ba => ba.Book)
                .SingleOrDefaultAsync();

            if (author == null)
            {
                return NotFound(); // Return 404 Not Found if the author with the chosen ID is not found
            }

            // Create a list of BookDTO objects for the author's books
            var booksDTO = author.BookAuthors.Select(ba => new BookDTO
            {
                Id = ba.Book.Id,
                Title = ba.Book.Title,
                Description = ba.Book.Description,
                Rating = ba.Book.Rating,
                DateAdded = ba.Book.DateAdded,
                Available = ba.Book.available,
                // Optionally, you can include other book properties if needed
            }).ToList();

            // Create an AuthorDTO object for the specific author
            var authorDTO = new AuthorDTO
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                BirthDate = author.BirthDate,
                // Optionally, you can include other author properties if needed

                // Populate the Books collection for the chosen author
                Books = booksDTO
            };

            return Ok(authorDTO); // Return the specific author with their corresponding books
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    
    
    
    
    //TODO: ავტორის დამატება
    
    //TODO: ავტორის რედაქტირება
    
    
}