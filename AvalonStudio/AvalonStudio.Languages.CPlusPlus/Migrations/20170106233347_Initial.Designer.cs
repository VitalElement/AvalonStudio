﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AvalonStudio.Languages.CPlusPlus.ProjectDatabase;

namespace AvalonStudio.Languages.CPlusPlus.Migrations
{
    [DbContext(typeof(ProjectContext))]
    [Migration("20170106233347_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SourceFile", b =>
                {
                    b.Property<int>("SourceFileId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("RelativePath");

                    b.HasKey("SourceFileId");

                    b.HasIndex("RelativePath")
                        .IsUnique();

                    b.ToTable("SourceFiles");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Symbol", b =>
                {
                    b.Property<int>("SymbolId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Column");

                    b.Property<int>("Line");

                    b.Property<int?>("SymbolReferenceId");

                    b.HasKey("SymbolId");

                    b.HasIndex("SymbolReferenceId");

                    b.ToTable("Symbols");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SymbolReference", b =>
                {
                    b.Property<int>("SymbolReferenceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DefinitionForeignKey");

                    b.Property<string>("Reference");

                    b.HasKey("SymbolReferenceId");

                    b.HasIndex("DefinitionForeignKey")
                        .IsUnique();

                    b.HasIndex("Reference")
                        .IsUnique();

                    b.ToTable("UniqueReferences");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Symbol", b =>
                {
                    b.HasOne("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SymbolReference", "SymbolReference")
                        .WithMany("Symbols")
                        .HasForeignKey("SymbolReferenceId");
                });

            modelBuilder.Entity("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SymbolReference", b =>
                {
                    b.HasOne("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.Symbol", "Definition")
                        .WithOne()
                        .HasForeignKey("AvalonStudio.Languages.CPlusPlus.ProjectDatabase.SymbolReference", "DefinitionForeignKey");
                });
        }
    }
}
