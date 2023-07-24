using Business.DTOs;

namespace Database.Repo.Interfaces;

public interface IAuthorRepository
{
    Task<List<AuthorDTO>> GetAuthors();
    Task<AuthorDTO> GetAuthorById(int id);
    Task AddAuthor(AddAuthorDTO addAuthorDto);
    Task EditAuthor(EditAuthorDTO editAuthorDTO);
    Task DeleteAuthorById(int authorId);
}