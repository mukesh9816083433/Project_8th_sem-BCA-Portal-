using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Notice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Semesters",
                table: "Notice");

            migrationBuilder.AddColumn<int>(
                name: "NoticeId",
                table: "Semesters",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoticeId1",
                table: "Semesters",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_NoticeId",
                table: "Semesters",
                column: "NoticeId");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_NoticeId1",
                table: "Semesters",
                column: "NoticeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_Notice_NoticeId",
                table: "Semesters",
                column: "NoticeId",
                principalTable: "Notice",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Semesters_Notice_NoticeId1",
                table: "Semesters",
                column: "NoticeId1",
                principalTable: "Notice",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_Notice_NoticeId",
                table: "Semesters");

            migrationBuilder.DropForeignKey(
                name: "FK_Semesters_Notice_NoticeId1",
                table: "Semesters");

            migrationBuilder.DropIndex(
                name: "IX_Semesters_NoticeId",
                table: "Semesters");

            migrationBuilder.DropIndex(
                name: "IX_Semesters_NoticeId1",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "NoticeId",
                table: "Semesters");

            migrationBuilder.DropColumn(
                name: "NoticeId1",
                table: "Semesters");

            migrationBuilder.AddColumn<string>(
                name: "Semesters",
                table: "Notice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
