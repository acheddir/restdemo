using BookStore.Services.Dto;

namespace BookStore.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookResponse>()
                .ForMember(br => br.Topic,
                    o => o.MapFrom(b => b.Topic != null ? b.Topic.Title : string.Empty));

            CreateMap<CreateBookCommand, Book>();
        }
    }
}
