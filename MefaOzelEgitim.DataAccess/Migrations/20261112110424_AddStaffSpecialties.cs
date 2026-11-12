using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MefaOzelEgitim.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffSpecialties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Specialties",
                table: "Staffs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Specialties",
                table: "Staffs");
        }
    }
}
