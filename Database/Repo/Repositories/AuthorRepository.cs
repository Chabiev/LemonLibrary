using Business.DTOs;
using Database.Repo.Interfaces;

namespace Database.Repo.Repositories;

public class AuthorRepository : IAuthorRepository
{
    public Task<List<AuthorDTO>> GetAuthors()
    {
        throw new NotImplementedException();
    }

    public Task<AuthorDTO> GetAuthorById(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAuthor(AddAuthorDTO addAuthorDto)
    {
        throw new NotImplementedException();
    }

    public Task EditAuthor(EditAuthorDTO editAuthorDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAuthorById(int authorId)
    {
        throw new NotImplementedException();
    }
}