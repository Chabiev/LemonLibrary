using Lemon_Library.Data;
using Lemon_Library.DTOs;
using Lemon_Library.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lemon_Library.Controllers;

public class BooksController : BaseApiController
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context)
    {
        _context = context;
    }
    
    //TODO: წიგნების სრული სიის წამოღება
    
    
    // GET: api/books
    [HttpGet()]
    public async Task<IActionResult> GetBooks()
    {
        try
        {
            var books = await _context.Books
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .ToListAsync();

            // Map the entity objects to DTO objects
            var bookDTOs = books.Select(book => new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Rating = book.Rating,
                DateAdded = book.DateAdded,
                Available = book.available,

                // Populate the Authors collection for each book
                Authors = book.BookAuthors.Select(ba => new AuthorDTO
                {
                    Id = ba.Author.Id,
                    FirstName = ba.Author.FirstName,
                    LastName = ba.Author.LastName,
                    BirthDate = ba.Author.BirthDate,
                    // Optionally, you can include other author properties if needed
                }).ToList()
            }).ToList();

            return Ok(bookDTOs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    
    //TODO: დეტალების წამოღება
    
    // GET: api/books/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        try
        {
            // Fetch the specific book with the chosen ID from the database
            var book = await _context.Books
                .Where(b => b.Id == id)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .SingleOrDefaultAsync();

            if (book == null)
            {
                return NotFound(); // Return 404 Not Found if the book with the chosen ID is not found
            }

            // Map the entity object to a DTO object
            var bookDTO = new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Rating = book.Rating,
                DateAdded = book.DateAdded,
                Available = book.available,

                // Populate the Authors collection for the chosen book
                Authors = book.BookAuthors.Select(ba => new AuthorDTO
                {
                    Id = ba.Author.Id,
                    FirstName = ba.Author.FirstName,
                    LastName = ba.Author.LastName,
                    BirthDate = ba.Author.BirthDate,
                    // Optionally, you can include other author properties if needed
                }).ToList()
            };

            return Ok(bookDTO); // Return the specific book with its corresponding authors
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    
    
    
    //TODO: წიგნის დამატება
    
    // POST: api/books
        [HttpPost("add")]
        public async Task<IActionResult> AddBookWithAuthor(BookAuthorDTO bookAuthorDTO)
        {
            if (ModelState.IsValid)
            {
                // Check if the provided Author exists in the database (case-insensitive)
                Author existingAuthor = await _context.Authors.FirstOrDefaultAsync(a =>
                    a.FirstName.ToLower() == bookAuthorDTO.FirstName.ToLower() &&
                    a.LastName.ToLower() == bookAuthorDTO.LastName.ToLower());

                // Check if a book with the same title exists in the database (case-insensitive)
                bool titleExists = await _context.Books.AnyAsync(b =>
                    b.Title.ToLower() == bookAuthorDTO.Title.ToLower());

                if (titleExists)
                {
                    return BadRequest("A book with the same title already exists.");
                }

                if (existingAuthor == null)
                {
                    // Create a new author
                    existingAuthor = new Author
                    {
                        FirstName = bookAuthorDTO.FirstName,
                        LastName = bookAuthorDTO.LastName,
                        BirthDate = bookAuthorDTO.BirthDate
                    };
                }

                // Create a new book
                var book = new Book
                {
                    Title = bookAuthorDTO.Title,
                    Description = bookAuthorDTO.Description,
                    Rating = bookAuthorDTO.Rating,
                    DateAdded = bookAuthorDTO.DateAdded,
                    available = bookAuthorDTO.Available
                };

                // Associate the book with the author
                book.BookAuthors = new[]
                {
                    new BookAuthor { Author = existingAuthor }
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return Ok("Book added successfully.");
            }

            return BadRequest("Invalid book data.");
        }
        
        
    //TODO: წიგნის რედაქტირება
    
    //TODO: გატანა დაბრუნება
    
    
    
}