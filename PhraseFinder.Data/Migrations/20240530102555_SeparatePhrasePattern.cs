using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhraseFinder.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeparatePhrasePattern : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locuciones_y_expresiones_Patron",
                table: "Locuciones_y_expresiones");

            migrationBuilder.DropColumn(
                name: "Patron",
                table: "Locuciones_y_expresiones");

            migrationBuilder.DropColumn(
                name: "Variante",
                table: "Locuciones_y_expresiones");

            migrationBuilder.CreateTable(
                name: "Patrones",
                columns: table => new
                {
                    ID_Patron = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Variante = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Patron = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Palabra_base = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ID_Locucion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patrones", x => x.ID_Patron);
                    table.ForeignKey(
                        name: "FK_Patrones_Locuciones_y_expresiones_ID_Locucion",
                        column: x => x.ID_Locucion,
                        principalTable: "Locuciones_y_expresiones",
                        principalColumn: "ID_Locucion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patrones_ID_Locucion",
                table: "Patrones",
                column: "ID_Locucion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patrones");

            migrationBuilder.AddColumn<string>(
                name: "Patron",
                table: "Locuciones_y_expresiones",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Variante",
                table: "Locuciones_y_expresiones",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Locuciones_y_expresiones_Patron",
                table: "Locuciones_y_expresiones",
                column: "Patron",
                unique: true);
        }
    }
}
