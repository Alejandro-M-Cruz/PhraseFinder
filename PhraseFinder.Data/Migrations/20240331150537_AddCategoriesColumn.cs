using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhraseFinder.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categorías",
                table: "Expresiones y locuciones",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categorías",
                table: "Expresiones y locuciones");
        }
    }
}
