using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "first_name",
                table: "Volunteers",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "last_name",
                table: "Volunteers",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "middle_name",
                table: "Volunteers",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "pets_id",
                table: "Pets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_pets_pets_id",
                table: "Pets",
                column: "pets_id");

            migrationBuilder.AddForeignKey(
                name: "fk_pets_volunteers_pets_id",
                table: "Pets",
                column: "pets_id",
                principalTable: "Volunteers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_pets_volunteers_pets_id",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "ix_pets_pets_id",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "first_name",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "last_name",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "middle_name",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "pets_id",
                table: "Pets");
        }
    }
}
