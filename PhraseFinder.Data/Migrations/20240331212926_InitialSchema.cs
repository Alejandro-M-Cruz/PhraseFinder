using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhraseFinder.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diccionarios",
                columns: table => new
                {
                    ID_Diccionario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "longchar", maxLength: 1000, nullable: true),
                    Formato = table.Column<string>(type: "longchar", nullable: false),
                    Ruta_fichero = table.Column<string>(type: "longchar", maxLength: 32767, nullable: false),
                    Fecha_creacion = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "Now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diccionarios", x => x.ID_Diccionario);
                });

            migrationBuilder.CreateTable(
                name: "Locuciones_y_expresiones",
                columns: table => new
                {
                    ID_Locucion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Locucion = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Palabra_base = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Variante = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Patron = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Categorias = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Revisado = table.Column<bool>(type: "smallint", nullable: false),
                    ID_Diccionario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locuciones_y_expresiones", x => x.ID_Locucion);
                    table.ForeignKey(
                        name: "FK_Locuciones_y_expresiones_Diccionarios_ID_Diccionario",
                        column: x => x.ID_Diccionario,
                        principalTable: "Diccionarios",
                        principalColumn: "ID_Diccionario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Definiciones",
                columns: table => new
                {
                    ID_Definicion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Definicion = table.Column<string>(type: "longchar", maxLength: 1000, nullable: false),
                    ID_Locucion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definiciones", x => x.ID_Definicion);
                    table.ForeignKey(
                        name: "FK_Definiciones_Locuciones_y_expresiones_ID_Locucion",
                        column: x => x.ID_Locucion,
                        principalTable: "Locuciones_y_expresiones",
                        principalColumn: "ID_Locucion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ejemplos",
                columns: table => new
                {
                    ID_Ejemplo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Ejemplo = table.Column<string>(type: "longchar", maxLength: 1000, nullable: false),
                    ID_Definicion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejemplos", x => x.ID_Ejemplo);
                    table.ForeignKey(
                        name: "FK_Ejemplos_Definiciones_ID_Definicion",
                        column: x => x.ID_Definicion,
                        principalTable: "Definiciones",
                        principalColumn: "ID_Definicion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Definiciones_ID_Locucion",
                table: "Definiciones",
                column: "ID_Locucion");

            migrationBuilder.CreateIndex(
                name: "IX_Ejemplos_ID_Definicion",
                table: "Ejemplos",
                column: "ID_Definicion");

            migrationBuilder.CreateIndex(
                name: "IX_Locuciones_y_expresiones_ID_Diccionario",
                table: "Locuciones_y_expresiones",
                column: "ID_Diccionario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ejemplos");

            migrationBuilder.DropTable(
                name: "Definiciones");

            migrationBuilder.DropTable(
                name: "Locuciones_y_expresiones");

            migrationBuilder.DropTable(
                name: "Diccionarios");
        }
    }
}
