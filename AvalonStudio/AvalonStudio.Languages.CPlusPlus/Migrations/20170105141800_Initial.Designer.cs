using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AvalonStudio.Languages.CPlusPlus.ProjectDatabase;

namespace AvalonStudio.Languages.CPlusPlus.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20170105141800_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SourceFiles", b =>
                {
                    b.Property<int>("SourceFilesId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("RelativePath");

                    b.HasKey("SourceFilesId");

                    b.ToTable("SourceFiles");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Symbol", b =>
                {
                    b.Property<int>("SymbolId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Column");

                    b.Property<int>("Line");

                    b.Property<int?>("USRSymbolReferenceId");

                    b.HasKey("SymbolId");

                    b.HasIndex("USRSymbolReferenceId");

                    b.ToTable("Symbols");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SymbolReference", b =>
                {
                    b.Property<int>("SymbolReferenceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DefinitionSymbolId");

                    b.Property<string>("Reference");

                    b.HasKey("SymbolReferenceId");

                    b.HasIndex("DefinitionSymbolId");

                    b.HasIndex("Reference")
                        .IsUnique();

                    b.ToTable("UniqueReferences");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Symbol", b =>
                {
                    b.HasOne("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SymbolReference", "USR")
                        .WithMany("Symbols")
                        .HasForeignKey("USRSymbolReferenceId");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SymbolReference", b =>
                {
                    b.HasOne("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Symbol", "Definition")
                        .WithMany()
                        .HasForeignKey("DefinitionSymbolId");
                });
        }
    }
}
