using BookStore.Services.Dto;

namespace BookStore.Services
{
    public interface IBookService
    {
        Task<List<BookResponse>> GetBooksAsync();
        Task<BookResponse> CreateBook(CreateBookCommand command);
        Task<BookResponse> GetBookById(int id);
        Task<int> RemoveBook(int id);
        Task<BookResponse> UpdateBook(int id, UpdateBookCommand command);
    }

    public class BookService : IBookService
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public BookService(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookResponse> CreateBook(CreateBookCommand command)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == command.Author);
            var topic = await _context.Topics.FirstOrDefaultAsync(t => t.Title == command.Topic);

            var book = new Book
            {
                Title = command.Title,
                Year = command.Year,
                TopicId = topic.Id,
                AuthorId = author.Id
            };

            _context.Books.Add(book);

            await _context.SaveChangesAsync();

            return new BookResponse(book.Title, book.Year, book.Topic.Title);
        }

        public async Task<BookResponse?> GetBookById(int id)
        {
            var book = await _context.Books.Include(b => b.Topic).FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return null;

            return new BookResponse(book.Title, book.Year, book.Topic.Title);
        }

        public async Task<List<BookResponse>> GetBooksAsync()
        {
            var bookEntities = await _context.Books
                .Include(b => b.Topic)
                .Select(b => new BookResponse(b.Title, b.Year, b.Topic.Title))
                .ToListAsync();

            return bookEntities;
        }

        public async Task<int> RemoveBook(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return 0;

            _context.Books.Remove(book);

            return await _context.SaveChangesAsync();
        }

        public async Task<BookResponse?> UpdateBook(int id, UpdateBookCommand command)
        {
            var book = await _context.Books.Include(b => b.Topic).FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return null;

            book.Title = command.Title;
            book.Year = command.Year;

            await _context.SaveChangesAsync();

            return new BookResponse(book.Title, book.Year, book.Topic.Title);
        }
    }
}
