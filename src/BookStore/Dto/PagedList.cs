namespace BookStore.Dto
{
    public class PagedList<T>
    {
        public int TotalCount { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }

        public IList<T> Items { get; set; }

        public PagedList(List<T> items, int count, int page, int size)
        {
            TotalCount = count;
            Size = size;
            Page = page;

            TotalPages = (int) Math.Ceiling(count / (double) size);

            Items = items;
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> querySource, int page, int size)
        {
            var count = querySource.Count();

            var items = await querySource
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return new PagedList<T>(items, count, page, size);
        }
    }
}
