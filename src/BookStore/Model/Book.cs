using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Model;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public required string ISBN { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Year { get; set; }

    public bool IsDeleted { get; set; }
}