using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AvalonStudio.Languages.CPlusPlus.ProjectDatabase;

namespace AvalonStudio.Languages.CPlusPlus.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20170104161612_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Declaration", b =>
                {
                    b.Property<int>("DeclarationId")
                        .ValueGeneratedOnAdd();

                    b.HasKey("DeclarationId");

                    b.ToTable("Declarations");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Definition", b =>
                {
                    b.Property<int>("DefinitionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("UniqueSymbolReference");

                    b.HasKey("DefinitionId");

                    b.ToTable("Definitions");
                });

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

                    b.Property<int?>("DefinitionId");

                    b.Property<int>("Line");

                    b.Property<int?>("USRSymbolReferenceId");

                    b.HasKey("SymbolId");

                    b.HasIndex("DefinitionId");

                    b.HasIndex("USRSymbolReferenceId");

                    b.ToTable("Symbols");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SymbolReference", b =>
                {
                    b.Property<int>("SymbolReferenceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Reference");

                    b.HasKey("SymbolReferenceId");

                    b.HasIndex("Reference")
                        .IsUnique();

                    b.ToTable("UniqueReferences");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Symbol", b =>
                {
                    b.HasOne("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Definition", "Definition")
                        .WithMany()
                        .HasForeignKey("DefinitionId");

                    b.HasOne("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SymbolReference", "USR")
                        .WithMany()
                        .HasForeignKey("USRSymbolReferenceId");
                });
        }
    }
}
