using Microsoft.AspNetCore.Http;

namespace Business.DTOs;

public class EditBookDTO
{
    // Book properties
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    
    public IFormFile ImageFile { get; set; }
    public double Rating { get; set; }

    // Author properties
    public int? AuthorId { get; set; } // Nullable, as it will be null when creating a new author
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}