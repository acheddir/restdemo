namespace BookStore.Dto
{
    public record BookResponse(string Isbn, string Title, string Year, AuthorResponse Author, TopicResponse Topic);
}
