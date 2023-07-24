namespace Business.DTOs;

public class BookAuthorDTO
{

    // Book properties
    public string Title { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public DateTime DateAdded { get; set; }
    public bool Available { get; set; }

    // Author properties
    public int? AuthorId { get; set; } // Nullable, as it will be null when creating a new author
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    
    
    // public byte[] Image { get; set; }
}

