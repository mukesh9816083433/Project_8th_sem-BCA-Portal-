using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingLoginCodeColumnInCampuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoginCode",
                table: "Campuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Campuses",
                keyColumn: "CampusID",
                keyValue: 1,
                column: "LoginCode",
                value: 11111);

            migrationBuilder.UpdateData(
                table: "Campuses",
                keyColumn: "CampusID",
                keyValue: 2,
                column: "LoginCode",
                value: 22222);

            migrationBuilder.UpdateData(
                table: "Campuses",
                keyColumn: "CampusID",
                keyValue: 3,
                column: "LoginCode",
                value: 33333);

            migrationBuilder.UpdateData(
                table: "Campuses",
                keyColumn: "CampusID",
                keyValue: 4,
                column: "LoginCode",
                value: 44444);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginCode",
                table: "Campuses");
        }
    }
}
