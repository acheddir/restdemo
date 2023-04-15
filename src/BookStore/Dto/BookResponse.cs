using Sieve.Attributes;

namespace BookStore.Dto
{
    public class BookResponse
    {
        public BookResponse(string isbn, string title, string year)
        {
            ISBN = isbn;
            Title = title;
            Year = year;
        }

        
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        public string Year { get; set; }
    }
        
}
