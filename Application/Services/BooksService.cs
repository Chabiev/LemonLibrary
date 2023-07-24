using Application.Interfaces;
using AutoMapper;
using Database.Repo.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace Application.Services;
using Database.Data;
using Business.DTOs;
using Database.Entities;

public class BooksService : IBooksService
{
    private readonly LibraryContext _context;
    private readonly IBookRepository _bookRepository;
    private readonly IMapper _mapper;

    public BooksService(LibraryContext context, IBookRepository bookRepository, IMapper mapper)
    {
        _context = context;
        _bookRepository = bookRepository;
        _mapper = mapper;
    }

    public async Task<List<BookDTO>> GetBooks()
    {
        var books = await _bookRepository.GetBooks();
        
        var bookDTOs = _mapper.Map<List<BookDTO>>(books);
        
        return bookDTOs;
    }

    public async Task<BookDTO> GetBookById(int id)
    {
        var book = await _bookRepository.GetBookById(id);

        var bookDTO = _mapper.Map<BookDTO>(book);

        return bookDTO; 
    }

    public async Task AddBookWithAuthor(BookAuthorDTO bookAuthorDTO, IFormFile imageFile)
    {
        Author existingAuthor = await _bookRepository.CheckBook(bookAuthorDTO);
        

        if (existingAuthor == null)
        {
            existingAuthor = new Author
            {
                FirstName = bookAuthorDTO.FirstName,
                LastName = bookAuthorDTO.LastName,
                BirthDate = bookAuthorDTO.BirthDate
            };
        }
        
        byte[] imageByteArray = null;
        if (imageFile != null && imageFile.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageByteArray = memoryStream.ToArray();
            }
        }

        var book = new Book
        {
            Title = bookAuthorDTO.Title,
            Description = bookAuthorDTO.Description,
            Rating = bookAuthorDTO.Rating,
            DateAdded = bookAuthorDTO.DateAdded,
            available = bookAuthorDTO.Available,
            Image = imageByteArray
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
        await _bookRepository.UpdateBookStatus(bookId);
    }

    public async Task DeleteBookById(int bookId)
    {
        await _bookRepository.DeleteBookById(bookId);
    }
}