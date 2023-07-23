using Business.DTOs;

namespace Application.Interfaces;

public interface IBooksService
{
    Task<List<BookDTO>> GetBooks();

    Task<BookDTO> GetBookById(int id);

    Task AddBookWithAuthor(BookAuthorDTO bookAuthorDTO);

    Task EditBook(EditBookDTO editBookDTO);

    Task ToggleBookStatus(int bookId);

    Task DeleteBookById(int bookId);
}