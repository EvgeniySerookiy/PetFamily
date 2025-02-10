﻿// <auto-generated />
using System;
using System.Collections.Generic;
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
    [Migration("20250209144255_Update17")]
    partial class Update17
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

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_of_birth");

                    b.ComplexProperty<Dictionary<string, object>>("BreedId", "PetFamily.Domain.PetContext.Pet.BreedId#BreedId", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Color", "PetFamily.Domain.PetContext.Pet.Color#NotEmptyString", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("character varying(30)")
                                .HasColumnName("color");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.Domain.PetContext.Pet.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("character varying(500)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("IsNeutered", "PetFamily.Domain.PetContext.Pet.IsNeutered#NeuteredStatus", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<bool>("Value")
                                .HasColumnType("boolean")
                                .HasColumnName("is_neutered");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("IsVaccinated", "PetFamily.Domain.PetContext.Pet.IsVaccinated#RabiesVaccinationStatus", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<bool>("Value")
                                .HasColumnType("boolean")
                                .HasColumnName("is_vaccinated");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Name", "PetFamily.Domain.PetContext.Pet.Name#NotEmptyString", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("character varying(30)")
                                .HasColumnName("name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("OwnerPhoneNumber", "PetFamily.Domain.PetContext.Pet.OwnerPhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("owner_phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PetAddress", "PetFamily.Domain.PetContext.Pet.PetAddress#Address", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Apartment")
                                .HasMaxLength(30)
                                .HasColumnType("character varying(30)")
                                .HasColumnName("apartment");

                            b1.ComplexProperty<Dictionary<string, object>>("Building", "PetFamily.Domain.PetContext.Pet.PetAddress#Address.Building#NotEmptyString", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)")
                                        .HasColumnName("building");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("City", "PetFamily.Domain.PetContext.Pet.PetAddress#Address.City#NotEmptyString", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)")
                                        .HasColumnName("city");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("Region", "PetFamily.Domain.PetContext.Pet.PetAddress#Address.Region#NotEmptyString", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)")
                                        .HasColumnName("region");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("Street", "PetFamily.Domain.PetContext.Pet.PetAddress#Address.Street#NotEmptyString", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("Value")
                                        .IsRequired()
                                        .HasMaxLength(30)
                                        .HasColumnType("character varying(30)")
                                        .HasColumnName("street");
                                });
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PetHealthInformation", "PetFamily.Domain.PetContext.Pet.PetHealthInformation#PetHealthInformation", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("character varying(500)")
                                .HasColumnName("pet_health_information");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Size", "PetFamily.Domain.PetContext.Pet.Size#Size", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Height")
                                .HasColumnType("integer")
                                .HasColumnName("height");

                            b1.Property<int>("Weight")
                                .HasColumnType("integer")
                                .HasColumnName("weight");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("SpeciesId", "PetFamily.Domain.PetContext.Pet.SpeciesId#SpeciesId", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("species_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Title", "PetFamily.Domain.PetContext.Pet.Title#Title", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(70)
                                .HasColumnType("character varying(70)")
                                .HasColumnName("title");
                        });

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
