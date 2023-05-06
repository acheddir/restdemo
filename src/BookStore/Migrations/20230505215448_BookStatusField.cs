using BookStore.Model;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class BookStatusField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:book_status", "draft,in_review,published,blocked");

            migrationBuilder.AddColumn<BookStatus>(
                name: "Status",
                table: "Books",
                type: "book_status",
                nullable: false,
                defaultValue: BookStatus.Draft);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Books");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:book_status", "draft,in_review,published,blocked");
        }
    }
}
