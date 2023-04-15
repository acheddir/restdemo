using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sieve.Attributes;
using System.Xml.Linq;

namespace BookStore.Model;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true, Name = "isbn")]
    public required string ISBN { get; set; }

    [Required]
    [Sieve(CanFilter = true, CanSort = true, Name = "title")]
    public required string Title { get; set; }

    [Required]
    public required string Year { get; set; }

    public bool IsDeleted { get; set; }

    public int TopicId { get; set; }
    public Topic? Topic { get; set; }

    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
}