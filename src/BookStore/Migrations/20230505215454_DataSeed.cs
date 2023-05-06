using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
            table: "Topics",
            columns: new[] { "Id", "Title" },
            values: new object[] { 2001, "ASP.NET Core" });

            migrationBuilder.InsertData(
            table: "Topics",
            columns: new[] { "Id", "Title" },
            values: new object[] { 2002, ".NET" });

            migrationBuilder.InsertData(
            table: "Topics",
            columns: new[] { "Id", "Title" },
            values: new object[] { 2003, "Software Architecture" });

            migrationBuilder.InsertData(
            table: "Authors",
            columns: new[] { "Id", "Name" },
            values: new object[] { 1001, "Robert C. Martin" });

            migrationBuilder.InsertData(
            table: "Authors",
            columns: new[] { "Id", "Name" },
            values: new object[] { 1002, "Mark J. Price" });

            migrationBuilder.InsertData(
            table: "Authors",
            columns: new[] { "Id", "Name" },
            values: new object[] { 1003, "John P. Smith" });

            migrationBuilder.InsertData(
            table: "Authors",
            columns: new[] { "Id", "Name" },
            values: new object[] { 1004, "Christian Wenz" });

            migrationBuilder.InsertData(
            table: "Authors",
            columns: new[] { "Id", "Name" },
            values: new object[] { 1005, "John J. Geewax" });

            migrationBuilder.InsertData(
            table: "Authors",
            columns: new[] { "Id", "Name" },
            values: new object[] { 1006, "Andrew Lock" });

            migrationBuilder.InsertData(
            table: "Books",
            columns: new[] { "Id", "ISBN", "Title", "Year", "AuthorId", "TopicId", "IsDeleted", "Status" },
            values: new object[] { 1, "9781801813433", "Apps & Services with .NET 7", "2022", 1002, 2002, false, BookStatus.Published });

            migrationBuilder.InsertData(
            table: "Books",
            columns: new[] { "Id", "ISBN", "Title", "Year", "AuthorId", "TopicId", "IsDeleted", "Status" },
            values: new object[] { 2, "9781803237800", "C# 11 and .NET 7 – Modern Cross-Platform Development Fundamentals", "2022", 1002, 2002, false, BookStatus.Published });

            migrationBuilder.InsertData(
            table: "Books",
            columns: new[] { "Id", "ISBN", "Title", "Year", "AuthorId", "TopicId", "IsDeleted", "Status" },
            values: new object[] { 3, "9781617298301", "ASP.NET Core in Action", "2021", 1006, 2001, false, BookStatus.Published });

            migrationBuilder.InsertData(
            table: "Books",
            columns: new[] { "Id", "ISBN", "Title", "Year", "AuthorId", "TopicId", "IsDeleted", "Status" },
            values: new object[] { 4, "9780136083238", "Clean Code: A Handbook of Agile Software Craftsmanship", "2008", 1001, 2003, false, BookStatus.Published });

            migrationBuilder.InsertData(
            table: "Books",
            columns: new[] { "Id", "ISBN", "Title", "Year", "AuthorId", "TopicId", "IsDeleted", "Status" },
            values: new object[] { 5, "9781617298363", "Entity Framework Core in Action", "2021", 1003, 2002, false, BookStatus.Published }); ;

            migrationBuilder.InsertData(
            table: "Books",
            columns: new[] { "Id", "ISBN", "Title", "Year", "AuthorId", "TopicId", "IsDeleted", "Status" },
            values: new object[] { 6, "9781633439986", "ASP.NET Core Security", "2022", 1004, 2001, false, BookStatus.Published });

            migrationBuilder.InsertData(
            table: "Books",
            columns: new[] { "Id", "ISBN", "Title", "Year", "AuthorId", "TopicId", "IsDeleted", "Status" },
            values: new object[] { 7, "9781617295850", "API Design Patterns", "2021", 1005, 2003, false, BookStatus.Published });

            migrationBuilder.InsertData(
            table: "Chapters",
            columns: new[] { "Id", "Name", "BookId" },
            values: new object[] { 3001, "Chapter 1", 1 });

            migrationBuilder.InsertData(
            table: "Chapters",
            columns: new[] { "Id", "Name", "BookId" },
            values: new object[] { 3002, "Chapter 2", 1 });

            migrationBuilder.InsertData(
            table: "Chapters",
            columns: new[] { "Id", "Name", "BookId" },
            values: new object[] { 3003, "Chapter 3", 1 });

            migrationBuilder.InsertData(
            table: "Chapters",
            columns: new[] { "Id", "Name", "BookId" },
            values: new object[] { 3004, "Chapter 4", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
            table: "Books",
            keyColumn: "Id",
            keyValue: 1);

            migrationBuilder.DeleteData(
            table: "Books",
            keyColumn: "Id",
            keyValue: 2);

            migrationBuilder.DeleteData(
            table: "Books",
            keyColumn: "Id",
            keyValue: 3);

            migrationBuilder.DeleteData(
            table: "Books",
            keyColumn: "Id",
            keyValue: 4);

            migrationBuilder.DeleteData(
            table: "Books",
            keyColumn: "Id",
            keyValue: 5);

            migrationBuilder.DeleteData(
            table: "Books",
            keyColumn: "Id",
            keyValue: 6);

            migrationBuilder.DeleteData(
            table: "Books",
            keyColumn: "Id",
            keyValue: 7);
        }
    }
}
