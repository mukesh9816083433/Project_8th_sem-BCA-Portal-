using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingTableForFirstSecondAndEntranceExamForms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SelectedSubjects",
                table: "EighthSemExamForms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentImagePath",
                table: "EighthSemExamForms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Collage",
                table: "EighthSemExamForms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "EntranceForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PaymentImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Collage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollageApproved = table.Column<bool>(type: "bit", nullable: false),
                    DeanApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntranceForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntranceForms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FirstSemExamForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SelectedSubjects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Collage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollageApproved = table.Column<bool>(type: "bit", nullable: false),
                    DeanApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirstSemExamForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirstSemExamForms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecondSemExamForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SelectedSubjects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Collage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollageApproved = table.Column<bool>(type: "bit", nullable: false),
                    DeanApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondSemExamForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecondSemExamForms_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntranceForms_UserId",
                table: "EntranceForms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FirstSemExamForms_UserId",
                table: "FirstSemExamForms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondSemExamForms_UserId",
                table: "SecondSemExamForms",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntranceForms");

            migrationBuilder.DropTable(
                name: "FirstSemExamForms");

            migrationBuilder.DropTable(
                name: "SecondSemExamForms");

            migrationBuilder.AlterColumn<string>(
                name: "SelectedSubjects",
                table: "EighthSemExamForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentImagePath",
                table: "EighthSemExamForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Collage",
                table: "EighthSemExamForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
