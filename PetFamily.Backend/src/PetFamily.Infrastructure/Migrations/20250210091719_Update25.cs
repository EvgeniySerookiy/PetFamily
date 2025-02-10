using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFamily.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "pets_rehomed",
                table: "Volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pets_seeking_home",
                table: "Volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "pets_under_treatment",
                table: "Volunteers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pets_rehomed",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "pets_seeking_home",
                table: "Volunteers");

            migrationBuilder.DropColumn(
                name: "pets_under_treatment",
                table: "Volunteers");
        }
    }
}
