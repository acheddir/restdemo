using NpgsqlTypes;

namespace BookStore.Model;

[PgName("book_status")]
public enum BookStatus
{
    [PgName("draft")]
    Draft,
    [PgName("in_review")]
    InReview,
    [PgName("published")]
    Published,
    [PgName("blocked")]
    Blocked
}