﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFamily.Infrastructure;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContex))]
    [Migration("20250209043214_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PetFamily.Domain.PetContext.Pet", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("Title");

                    b.HasKey("Id")
                        .HasName("pk_pets");

                    b.ToTable("Pets", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesContext.SpeciesEntities.Breed", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid?>("species_id")
                        .HasColumnType("uuid")
                        .HasColumnName("species_id");

                    b.HasKey("Id")
                        .HasName("pk_breeds");

                    b.HasIndex("species_id")
                        .HasDatabaseName("ix_breeds_species_id");

                    b.ToTable("Breeds", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesContext.SpeciesEntities.Species", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("Species", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.VolunteerContext.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("Volunteers", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesContext.SpeciesEntities.Breed", b =>
                {
                    b.HasOne("PetFamily.Domain.SpeciesContext.SpeciesEntities.Species", null)
                        .WithMany("Breeds")
                        .HasForeignKey("species_id")
                        .HasConstraintName("fk_breeds_species_species_id");
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesContext.SpeciesEntities.Species", b =>
                {
                    b.Navigation("Breeds");
                });
#pragma warning restore 612, 618
        }
    }
}
