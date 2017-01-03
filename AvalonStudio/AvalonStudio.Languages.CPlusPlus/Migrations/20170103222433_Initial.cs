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
                    SourceFilesId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastModified = table.Column<DateTime>(nullable: false),
                    RelativePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceFiles", x => x.SourceFilesId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SourceFiles");
        }
    }
}
