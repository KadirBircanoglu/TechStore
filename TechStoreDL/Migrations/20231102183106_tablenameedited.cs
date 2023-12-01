using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStoreDL.Migrations
{
    /// <inheritdoc />
    public partial class tablenameedited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactMessageTablo",
                table: "ContactMessageTablo");

            migrationBuilder.RenameTable(
                name: "ContactMessageTablo",
                newName: "ContactMessageTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactMessageTable",
                table: "ContactMessageTable",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactMessageTable",
                table: "ContactMessageTable");

            migrationBuilder.RenameTable(
                name: "ContactMessageTable",
                newName: "ContactMessageTablo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactMessageTablo",
                table: "ContactMessageTablo",
                column: "Id");
        }
    }
}
