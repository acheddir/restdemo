using Sieve.Models;

namespace BookStore.Services
{
    public interface IBookService
    {
        Task<List<BookResponse>> GetBooksAsync(SieveModel sieveModel);
        Task<List<BookResponse>> GetBooksByAuthorv2Async(int authorId);
        Task<List<string>> GetBooksByAuthorAsync(int authorId);
        Task<BookResponse> GetBookAsync(string isbn);
        Task<BookResponse> CreateBookAsync(CreateBookCommand command);
        Task<BookResponse> UpdateBookAsync(string isbn, UpdateBookCommand command);
        Task DeleteBookAsync(string isbn);
    }
}
