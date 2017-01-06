using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AvalonStudio.Languages.CPlusPlus.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SourceFiles",
                columns: table => new
                {
                    SourceFileId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastModified = table.Column<DateTime>(nullable: false),
                    RelativePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceFiles", x => x.SourceFileId);
                });

            migrationBuilder.CreateTable(
                name: "UniqueReferences",
                columns: table => new
                {
                    SymbolReferenceId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Reference = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueReferences", x => x.SymbolReferenceId);
                });

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    SymbolId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Column = table.Column<int>(nullable: false),
                    Line = table.Column<int>(nullable: false),
                    SymbolReferenceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.SymbolId);
                    table.ForeignKey(
                        name: "FK_Symbols_UniqueReferences_SymbolReferenceId",
                        column: x => x.SymbolReferenceId,
                        principalTable: "UniqueReferences",
                        principalColumn: "SymbolReferenceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SourceFiles_RelativePath",
                table: "SourceFiles",
                column: "RelativePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_SymbolReferenceId",
                table: "Symbols",
                column: "SymbolReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueReferences_Reference",
                table: "UniqueReferences",
                column: "Reference",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SourceFiles");

            migrationBuilder.DropTable(
                name: "Symbols");

            migrationBuilder.DropTable(
                name: "UniqueReferences");
        }
    }
}
