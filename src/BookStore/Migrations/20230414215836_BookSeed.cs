using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class BookSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "ISBN", "Title", "Year" },
                values: new object[] { 1, "9781803237824", "Mastering Minimal APIs in ASP.NET Core", "2022" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "ISBN", "Title", "Year" },
                values: new object[] { 2, "9781803237800", "C# 11 and .NET 7 – Modern Cross-Platform Development Fundamentals", "2022" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "ISBN", "Title", "Year" },
                values: new object[] { 3, "9781617298301", "ASP.NET Core in Action", "2021" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "ISBN", "Title", "Year" },
                values: new object[] { 4, "9780136083238", "Clean Code: A Handbook of Agile Software Craftsmanship", "2008" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "ISBN", "Title", "Year" },
                values: new object[] { 5, "9781617298363", "Entity Framework Core in Action", "2021" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "ISBN", "Title", "Year" },
                values: new object[] { 6, "9781633439986", "ASP.NET Core Security", "2022" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "ISBN", "Title", "Year" },
                values: new object[] { 7, "9781617295850", "API Design Patterns", "2021" });
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
