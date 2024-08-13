using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedCampusData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Campuses",
                columns: new[] { "CampusID", "Address", "CampusChief", "CampusName", "ContactInfo", "Province" },
                values: new object[,]
                {
                    { 1, "Bhadrapur, Jhapa", "Jiwan Pokhrel", "Mechi Multiple Campus", "9852655607", "Province 1" },
                    { 2, "Illam", "Dinanath Phuyal", "Mahendra Ratna Multiple Campus", "9812345678", "Province 1" },
                    { 3, "Lahan", "Tulsi Ram Pokhrel", "Jwala Prasad Syo Wai Devi Murarka Campus", "9812365855", "Province 2" },
                    { 4, "Bhaktapur", "Krishna Pokhrel", "Bhaktapur Multiple Campus", "990000045678", "Province 3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Campuses",
                keyColumn: "CampusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Campuses",
                keyColumn: "CampusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Campuses",
                keyColumn: "CampusID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Campuses",
                keyColumn: "CampusID",
                keyValue: 4);
        }
    }
}
