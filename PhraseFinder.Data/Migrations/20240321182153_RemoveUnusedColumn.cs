using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhraseFinder.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diccionarios",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Descripción = table.Column<string>(type: "longchar", maxLength: 1000, nullable: true),
                    Formato = table.Column<string>(type: "longchar", nullable: false),
                    Rutadelfichero = table.Column<string>(name: "Ruta del fichero", type: "longchar", maxLength: 500, nullable: false),
                    Fechadecreación = table.Column<DateTime>(name: "Fecha de creación", type: "datetime", nullable: false, defaultValueSql: "Now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diccionarios", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Expresiones y locuciones",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Expresiónolocución = table.Column<string>(name: "Expresión o locución", type: "varchar(255)", maxLength: 255, nullable: false),
                    Palabrabase = table.Column<string>(name: "Palabra base", type: "varchar(255)", maxLength: 255, nullable: false),
                    IDdediccionario = table.Column<int>(name: "ID de diccionario", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expresiones y locuciones", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Expresiones y locuciones_Diccionarios_ID de diccionario",
                        column: x => x.IDdediccionario,
                        principalTable: "Diccionarios",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Definiciones",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Definición = table.Column<string>(type: "longchar", maxLength: 1000, nullable: false),
                    IDdeexpresiónolocución = table.Column<int>(name: "ID de expresión o locución", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definiciones", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Definiciones_Expresiones y locuciones_ID de expresión o locu~",
                        column: x => x.IDdeexpresiónolocución,
                        principalTable: "Expresiones y locuciones",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ejemplos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Ejemplo = table.Column<string>(type: "longchar", maxLength: 1000, nullable: false),
                    IDdedefinición = table.Column<int>(name: "ID de definición", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ejemplos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ejemplos_Definiciones_ID de definición",
                        column: x => x.IDdedefinición,
                        principalTable: "Definiciones",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Definiciones_ID de expresión o locución",
                table: "Definiciones",
                column: "ID de expresión o locución");

            migrationBuilder.CreateIndex(
                name: "IX_Ejemplos_ID de definición",
                table: "Ejemplos",
                column: "ID de definición");

            migrationBuilder.CreateIndex(
                name: "IX_Expresiones y locuciones_ID de diccionario",
                table: "Expresiones y locuciones",
                column: "ID de diccionario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ejemplos");

            migrationBuilder.DropTable(
                name: "Definiciones");

            migrationBuilder.DropTable(
                name: "Expresiones y locuciones");

            migrationBuilder.DropTable(
                name: "Diccionarios");
        }
    }
}
