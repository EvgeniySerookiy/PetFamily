using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pet_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_species", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pets_rehomed = table.Column<int>(type: "integer", nullable: false),
                    pets_seeking_home = table.Column<int>(type: "integer", nullable: false),
                    pets_under_treatment = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    years_of_experience = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transfer_requisites_for_help_list = table.Column<string>(type: "jsonb", nullable: false),
                    transfer_social_network_list = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: true),
                    pet_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_breeds", x => x.id);
                    table.ForeignKey(
                        name: "fk_breeds_species_species_id",
                        column: x => x.species_id,
                        principalTable: "Species",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_of_birth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    date_of_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    pets_id = table.Column<Guid>(type: "uuid", nullable: true),
                    breed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    is_neutered = table.Column<bool>(type: "boolean", nullable: false),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    owner_phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    apartment = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    building = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    city = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    region = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    street = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    pet_health_information = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    pet_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    species_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deletion_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_pets_id",
                        column: x => x.pets_id,
                        principalTable: "Volunteers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_breeds_species_id",
                table: "Breeds",
                column: "species_id");

            migrationBuilder.CreateIndex(
                name: "ix_pets_pets_id",
                table: "Pets",
                column: "pets_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Breeds");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "Volunteers");
        }
    }
}
