namespace BookStore.Dto
{
    public record CreateBookCommand(string ISBN, string Title, string Year);
}
