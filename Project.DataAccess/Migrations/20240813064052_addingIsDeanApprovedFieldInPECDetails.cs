using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingIsDeanApprovedFieldInPECDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeanApproved",
                table: "PersonalDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeanApproved",
                table: "EducationalDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeanApproved",
                table: "ContactDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeanApproved",
                table: "PersonalDetails");

            migrationBuilder.DropColumn(
                name: "IsDeanApproved",
                table: "EducationalDetails");

            migrationBuilder.DropColumn(
                name: "IsDeanApproved",
                table: "ContactDetails");
        }
    }
}
