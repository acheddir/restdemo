namespace BookStore.Services.Dto
{
    public record CreateBookCommand(string Title, int Year, string Topic, string Author);
}
