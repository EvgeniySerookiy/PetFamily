﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PetFamily.Infrastructure;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContex))]
    partial class ApplicationDbContexModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime>("DateOfCreation")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_of_creation");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("status");

                    b.Property<Guid?>("pets_id")
                        .HasColumnType("uuid")
                        .HasColumnName("pets_id");

                    b.ComplexProperty<Dictionary<string, object>>("BreedId", "PetFamily.Domain.PetContext.Pet.BreedId#BreedId", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("breed_id");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Color", "PetFamily.Domain.PetContext.Pet.Color#Color", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
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

                            b1.Property<string>("Building")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("character varying(30)")
                                .HasColumnName("building");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("character varying(30)")
                                .HasColumnName("city");

                            b1.Property<string>("Region")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("character varying(30)")
                                .HasColumnName("region");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasMaxLength(30)
                                .HasColumnType("character varying(30)")
                                .HasColumnName("street");
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

                    b.ComplexProperty<Dictionary<string, object>>("PetName", "PetFamily.Domain.PetContext.Pet.PetName#PetName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("pet_name");
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

                    b.HasIndex("pets_id")
                        .HasDatabaseName("ix_pets_pets_id");

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

                    b.ComplexProperty<Dictionary<string, object>>("PetName", "PetFamily.Domain.SpeciesContext.SpeciesEntities.Breed.PetName#PetName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("pet_name");
                        });

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

                    b.ComplexProperty<Dictionary<string, object>>("PetName", "PetFamily.Domain.SpeciesContext.SpeciesEntities.Species.PetName#PetName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("pet_name");
                        });

                    b.HasKey("Id")
                        .HasName("pk_species");

                    b.ToTable("Species", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.VolunteerContext.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("PetsRehomed")
                        .HasColumnType("integer")
                        .HasColumnName("pets_rehomed");

                    b.Property<int>("PetsSeekingHome")
                        .HasColumnType("integer")
                        .HasColumnName("pets_seeking_home");

                    b.Property<int>("PetsUnderTreatment")
                        .HasColumnType("integer")
                        .HasColumnName("pets_under_treatment");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "PetFamily.Domain.VolunteerContext.Volunteer.Description#Description", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(500)
                                .HasColumnType("character varying(500)")
                                .HasColumnName("description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Email", "PetFamily.Domain.VolunteerContext.Volunteer.Email#Email", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FullName", "PetFamily.Domain.VolunteerContext.Volunteer.FullName#FullName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("first_name");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("last_name");

                            b1.Property<string>("MiddleName")
                                .HasMaxLength(50)
                                .HasColumnType("character varying(50)")
                                .HasColumnName("middle_name");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "PetFamily.Domain.VolunteerContext.Volunteer.PhoneNumber#PhoneNumber", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(20)
                                .HasColumnType("character varying(20)")
                                .HasColumnName("phone_number");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("YearsOfExperience", "PetFamily.Domain.VolunteerContext.Volunteer.YearsOfExperience#YearsOfExperience", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("integer")
                                .HasColumnName("years_of_experience");
                        });

                    b.HasKey("Id")
                        .HasName("pk_volunteers");

                    b.ToTable("Volunteers", (string)null);
                });

            modelBuilder.Entity("PetFamily.Domain.PetContext.Pet", b =>
                {
                    b.HasOne("PetFamily.Domain.VolunteerContext.Volunteer", null)
                        .WithMany("Pets")
                        .HasForeignKey("pets_id")
                        .HasConstraintName("fk_pets_volunteers_pets_id");
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesContext.SpeciesEntities.Breed", b =>
                {
                    b.HasOne("PetFamily.Domain.SpeciesContext.SpeciesEntities.Species", null)
                        .WithMany("Breeds")
                        .HasForeignKey("species_id")
                        .HasConstraintName("fk_breeds_species_species_id");
                });

            modelBuilder.Entity("PetFamily.Domain.VolunteerContext.Volunteer", b =>
                {
                    b.OwnsOne("PetFamily.Domain.VolunteerContext.VolunteerVO.TransferRequisitesForHelpsList", "TransferRequisitesForHelpsList", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("Volunteers");

                            b1.ToJson("TransferRequisitesForHelpsList");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("PetFamily.Domain.VolunteerContext.VolunteerVO.RequisitesForHelp", "RequisitesForHelps", b2 =>
                                {
                                    b2.Property<Guid>("TransferRequisitesForHelpsListVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("__synthesizedOrdinal")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("PaymentDetails")
                                        .IsRequired()
                                        .HasMaxLength(200)
                                        .HasColumnType("character varying(200)")
                                        .HasColumnName("payment_details");

                                    b2.Property<string>("Recipient")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)")
                                        .HasColumnName("recipient");

                                    b2.HasKey("TransferRequisitesForHelpsListVolunteerId", "__synthesizedOrdinal")
                                        .HasName("pk_volunteers");

                                    b2.ToTable("Volunteers");

                                    b2.WithOwner()
                                        .HasForeignKey("TransferRequisitesForHelpsListVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_transfer_requisites_for_helps_list_volunte");
                                });

                            b1.Navigation("RequisitesForHelps");
                        });

                    b.OwnsOne("PetFamily.Domain.VolunteerContext.VolunteerVO.TransferSocialNetworkList", "TransferSocialNetworkList", b1 =>
                        {
                            b1.Property<Guid>("VolunteerId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.HasKey("VolunteerId");

                            b1.ToTable("Volunteers");

                            b1.ToJson("TransferSocialNetworkList");

                            b1.WithOwner()
                                .HasForeignKey("VolunteerId")
                                .HasConstraintName("fk_volunteers_volunteers_id");

                            b1.OwnsMany("PetFamily.Domain.VolunteerContext.VolunteerVO.SocialNetwork", "SocialNetworks", b2 =>
                                {
                                    b2.Property<Guid>("TransferSocialNetworkListVolunteerId")
                                        .HasColumnType("uuid");

                                    b2.Property<int>("__synthesizedOrdinal")
                                        .ValueGeneratedOnAdd()
                                        .HasColumnType("integer");

                                    b2.Property<string>("NetworkAddress")
                                        .IsRequired()
                                        .HasMaxLength(200)
                                        .HasColumnType("character varying(200)")
                                        .HasColumnName("network_address");

                                    b2.Property<string>("NetworkName")
                                        .IsRequired()
                                        .HasMaxLength(100)
                                        .HasColumnType("character varying(100)")
                                        .HasColumnName("network_name");

                                    b2.HasKey("TransferSocialNetworkListVolunteerId", "__synthesizedOrdinal")
                                        .HasName("pk_volunteers");

                                    b2.ToTable("Volunteers");

                                    b2.WithOwner()
                                        .HasForeignKey("TransferSocialNetworkListVolunteerId")
                                        .HasConstraintName("fk_volunteers_volunteers_transfer_social_network_list_volunteer_id");
                                });

                            b1.Navigation("SocialNetworks");
                        });

                    b.Navigation("TransferRequisitesForHelpsList")
                        .IsRequired();

                    b.Navigation("TransferSocialNetworkList")
                        .IsRequired();
                });

            modelBuilder.Entity("PetFamily.Domain.SpeciesContext.SpeciesEntities.Species", b =>
                {
                    b.Navigation("Breeds");
                });

            modelBuilder.Entity("PetFamily.Domain.VolunteerContext.Volunteer", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
