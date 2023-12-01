using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStoreDL.Migrations
{
    /// <inheritdoc />
    public partial class isdeleted_col : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "ProductDiscountTable",
                newName: "IsFinished");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFinished",
                table: "ProductDiscountTable",
                newName: "IsDeleted");
        }
    }
}
