namespace BookStore.Model;

public class BookStoreContext : DbContext
{
    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Topic> Topics => Set<Topic>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>("IdSequence")
            .IncrementsBy(1000);

        modelBuilder.Entity<Author>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .UseHiLo("IdSequence");

        modelBuilder.Entity<Topic>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .UseHiLo("IdSequence");

        modelBuilder.Entity<Book>()
            .Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .UseHiLo("IdSequence");

        base.OnModelCreating(modelBuilder);
    }
}