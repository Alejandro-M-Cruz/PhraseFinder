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
                name: "PhraseDictionaries",
                columns: table => new
                {
                    PhraseDictionaryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Name = table.Column<string>(type: "longchar", nullable: false),
                    Description = table.Column<string>(type: "longchar", nullable: true),
                    Format = table.Column<int>(type: "integer", nullable: false),
                    Path = table.Column<string>(type: "longchar", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "Now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhraseDictionaries", x => x.PhraseDictionaryId);
                });

            migrationBuilder.CreateTable(
                name: "Phrases",
                columns: table => new
                {
                    PhraseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Name = table.Column<string>(type: "longchar", nullable: false),
                    RegExPattern = table.Column<string>(type: "longchar", nullable: true),
                    BaseWord = table.Column<string>(type: "longchar", nullable: false),
                    PhraseDictionaryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phrases", x => x.PhraseId);
                    table.ForeignKey(
                        name: "FK_Phrases_PhraseDictionaries_PhraseDictionaryId",
                        column: x => x.PhraseDictionaryId,
                        principalTable: "PhraseDictionaries",
                        principalColumn: "PhraseDictionaryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhraseDefinition",
                columns: table => new
                {
                    PhraseDefinitionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "longchar", nullable: false),
                    PhraseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhraseDefinition", x => x.PhraseDefinitionId);
                    table.ForeignKey(
                        name: "FK_PhraseDefinition_Phrases_PhraseId",
                        column: x => x.PhraseId,
                        principalTable: "Phrases",
                        principalColumn: "PhraseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhraseExample",
                columns: table => new
                {
                    PhraseExampleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Example = table.Column<string>(type: "longchar", nullable: false),
                    PhraseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhraseExample", x => x.PhraseExampleId);
                    table.ForeignKey(
                        name: "FK_PhraseExample_Phrases_PhraseId",
                        column: x => x.PhraseId,
                        principalTable: "Phrases",
                        principalColumn: "PhraseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhraseDefinition_PhraseId",
                table: "PhraseDefinition",
                column: "PhraseId");

            migrationBuilder.CreateIndex(
                name: "IX_PhraseExample_PhraseId",
                table: "PhraseExample",
                column: "PhraseId");

            migrationBuilder.CreateIndex(
                name: "IX_Phrases_PhraseDictionaryId",
                table: "Phrases",
                column: "PhraseDictionaryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhraseDefinition");

            migrationBuilder.DropTable(
                name: "PhraseExample");

            migrationBuilder.DropTable(
                name: "Phrases");

            migrationBuilder.DropTable(
                name: "PhraseDictionaries");
        }
    }
}
