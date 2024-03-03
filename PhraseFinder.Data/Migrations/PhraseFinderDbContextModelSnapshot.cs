﻿// <auto-generated />
using System;
using EntityFrameworkCore.Jet.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhraseFinder.Data;

#nullable disable

namespace PhraseFinder.Data.Migrations
{
    [DbContext(typeof(PhraseFinderDbContext))]
    partial class PhraseFinderDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BaseWord")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("PhraseDictionaryId")
                        .HasColumnType("integer");

                    b.Property<string>("RegExPattern")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("longchar");

                    b.HasKey("PhraseId");

                    b.HasIndex("PhraseDictionaryId");

                    b.ToTable("Phrases", (string)null);
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDefinition", b =>
                {
                    b.Property<int>("PhraseDefinitionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("longchar");

                    b.Property<int>("PhraseId")
                        .HasColumnType("integer");

                    b.HasKey("PhraseDefinitionId");

                    b.HasIndex("PhraseId");

                    b.ToTable("PhraseDefinition", (string)null);
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDictionary", b =>
                {
                    b.Property<int>("PhraseDictionaryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("Now()");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("longchar");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("longchar");

                    b.Property<int>("Format")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("PhraseDictionaryId");

                    b.ToTable("PhraseDictionaries", (string)null);
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseExample", b =>
                {
                    b.Property<int>("PhraseExampleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Jet:ValueGenerationStrategy", JetValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Example")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("longchar");

                    b.Property<int>("PhraseId")
                        .HasColumnType("integer");

                    b.HasKey("PhraseExampleId");

                    b.HasIndex("PhraseId");

                    b.ToTable("PhraseExample", (string)null);
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.Phrase", b =>
                {
                    b.HasOne("PhraseFinder.Domain.Models.PhraseDictionary", null)
                        .WithMany("Phrases")
                        .HasForeignKey("PhraseDictionaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseDefinition", b =>
                {
                    b.HasOne("PhraseFinder.Domain.Models.Phrase", null)
                        .WithMany("Definitions")
                        .HasForeignKey("PhraseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.PhraseExample", b =>
                {
                    b.HasOne("PhraseFinder.Domain.Models.Phrase", null)
                        .WithMany("Examples")
                        .HasForeignKey("PhraseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PhraseFinder.Domain.Models.Phrase", b =>
                {
                    b.Navigation("Definitions");

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
