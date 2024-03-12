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
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "longchar", maxLength: 1000, nullable: true),
                    Format = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "longchar", maxLength: 500, nullable: false),
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
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    RegExPattern = table.Column<string>(type: "longchar", maxLength: 2000, nullable: false),
                    BaseWord = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
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
                    Definition = table.Column<string>(type: "longchar", maxLength: 1000, nullable: false),
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
                name: "PhraseDefinitionExample",
                columns: table => new
                {
                    PhraseDefinitionExampleId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Jet:Identity", "1, 1"),
                    Example = table.Column<string>(type: "longchar", maxLength: 1000, nullable: false),
                    PhraseDefinitionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhraseDefinitionExample", x => x.PhraseDefinitionExampleId);
                    table.ForeignKey(
                        name: "FK_PhraseDefinitionExample_PhraseDefinition_PhraseDefinitionId",
                        column: x => x.PhraseDefinitionId,
                        principalTable: "PhraseDefinition",
                        principalColumn: "PhraseDefinitionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhraseDefinition_PhraseId",
                table: "PhraseDefinition",
                column: "PhraseId");

            migrationBuilder.CreateIndex(
                name: "IX_PhraseDefinitionExample_PhraseDefinitionId",
                table: "PhraseDefinitionExample",
                column: "PhraseDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Phrases_PhraseDictionaryId",
                table: "Phrases",
                column: "PhraseDictionaryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhraseDefinitionExample");

            migrationBuilder.DropTable(
                name: "PhraseDefinition");

            migrationBuilder.DropTable(
                name: "Phrases");

            migrationBuilder.DropTable(
                name: "PhraseDictionaries");
        }
    }
}
