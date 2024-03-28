using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhraseFinder.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhraseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Patrón",
                table: "Expresiones y locuciones",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Revisado",
                table: "Expresiones y locuciones",
                type: "smallint",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Variante",
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
                name: "Patrón",
                table: "Expresiones y locuciones");

            migrationBuilder.DropColumn(
                name: "Revisado",
                table: "Expresiones y locuciones");

            migrationBuilder.DropColumn(
                name: "Variante",
                table: "Expresiones y locuciones");
        }
    }
}
