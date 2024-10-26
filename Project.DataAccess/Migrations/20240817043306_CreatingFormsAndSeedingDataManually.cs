using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreatingFormsAndSeedingDataManually : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Forms",
                columns: new[] { "Id", "IsOpen", "Name" },
                values: new object[,]
                {
                    { 1, false, "Entrance Form" },
                    { 2, false, "Registration Form" },
                    { 3, false, "1st Sem Exam Form" },
                    { 4, false, "2nd Sem Exam Form" },
                    { 5, false, "3rd Sem Exam Form" },
                    { 6, false, "4th Sem Exam Form" },
                    { 7, false, "5th Sem Exam Form" },
                    { 8, false, "6th Sem Exam Form" },
                    { 9, false, "7th Sem Exam Form" },
                    { 10, false, "8th Sem Exam Form" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forms");
        }
    }
}
