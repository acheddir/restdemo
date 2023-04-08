using AutoMapper.QueryableExtensions;
using BookStore.Services.Dto;

namespace BookStore.Services
{
    public interface IBookService
    {
        Task<List<BookResponse>> GetBooksAsync();
        Task<BookResponse> CreateBook(CreateBookCommand command);
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
                Id = 2,
                Title = command.Title,
                Year = command.Year,
                TopicId = topic.Id,
                AuthorId = author.Id
            };

            _context.Books.Add(book);

            await _context.SaveChangesAsync();

            return _mapper.Map<BookResponse>(book);
        }

        public Task<List<BookResponse>> GetBooksAsync()
        {
            return _context.Books
                .Include(b => b.Topic)
                .ProjectTo<BookResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
