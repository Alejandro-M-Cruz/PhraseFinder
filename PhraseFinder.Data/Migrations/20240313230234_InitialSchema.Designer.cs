﻿// <auto-generated />
using System;
using EntityFrameworkCore.Jet.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhraseFinder.Data;

#nullable disable

namespace PhraseFinder.Data.Migrations
{
    [DbContext(typeof(PhraseFinderDbContext))]
    [Migration("20240313230234_InitialSchema")]
    partial class InitialSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PhraseFinder.Domain.Models.Phrase", b =>
                {
                    b.Property<int>("PhraseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BaseWord")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Palabra base");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Nombre");

                    b.Property<int>("PhraseDictionaryId")
                        .HasColumnType("integer")
                        .HasColumnName("ID de diccionario");

                    b.Property<string>("RegExPattern")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("longchar")
                        .HasColumnName("Expresión regular");

                    b.HasKey("PhraseId");

                    b.HasIndex("PhraseDictionaryId");

                    b.ToTable("Expresiones y locuciones");
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDefinition", b =>
                {
                    b.Property<int>("PhraseDefinitionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("longchar")
                        .HasColumnName("Definición");

                    b.Property<int>("PhraseId")
                        .HasColumnType("integer")
                        .HasColumnName("ID de expresión o locución");

                    b.HasKey("PhraseDefinitionId");

                    b.HasIndex("PhraseId");

                    b.ToTable("Definiciones");
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDefinitionExample", b =>
                {
                    b.Property<int>("PhraseDefinitionExampleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Example")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("longchar")
                        .HasColumnName("Ejemplo");

                    b.Property<int>("PhraseDefinitionId")
                        .HasColumnType("integer")
                        .HasColumnName("ID de definición");

                    b.HasKey("PhraseDefinitionExampleId");

                    b.HasIndex("PhraseDefinitionId");

                    b.ToTable("Ejemplos");
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDictionary", b =>
                {
                    b.Property<int>("PhraseDictionaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("ID")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("Fecha de creación")
                        .HasDefaultValueSql("Now()");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("longchar")
                        .HasColumnName("Descripción");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("longchar")
                        .HasColumnName("Ruta del fichero");

                    b.Property<string>("Format")
                        .IsRequired()
                        .HasColumnType("longchar")
                        .HasColumnName("Formato");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("Nombre");

                    b.HasKey("PhraseDictionaryId");

                    b.ToTable("Diccionarios");
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.Phrase", b =>
                {
                    b.HasOne("PhraseFinder.Domain.Models.PhraseDictionary", "PhraseDictionary")
                        .WithMany("Phrases")
                        .HasForeignKey("PhraseDictionaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PhraseDictionary");
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDefinition", b =>
                {
                    b.HasOne("PhraseFinder.Domain.Models.Phrase", null)
                        .WithMany("Definitions")
                        .HasForeignKey("PhraseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDefinitionExample", b =>
                {
                    b.HasOne("PhraseFinder.Domain.Models.PhraseDefinition", null)
                        .WithMany("Examples")
                        .HasForeignKey("PhraseDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.Phrase", b =>
                {
                    b.Navigation("Definitions");
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDefinition", b =>
                {
                    b.Navigation("Examples");
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDictionary", b =>
                {
                    b.Navigation("Phrases");
                });
#pragma warning restore 612, 618
        }
    }
}