namespace BookStore.Model;

public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int Year { get; set; }
    
    public int TopicId { get; set; }
    public Topic? Topic { get; set; }

    public int AuthorId {get; set;}
    public Author? Author { get; set; }
}