namespace BookStore.Dto
{
    public record BookResponse(string Isbn, string Title, string Year, string Status, AuthorResponse Author, TopicResponse Topic);
}
