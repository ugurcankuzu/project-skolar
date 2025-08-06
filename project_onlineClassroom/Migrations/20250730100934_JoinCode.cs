using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_onlineClassroom.Migrations
{
    /// <inheritdoc />
    public partial class JoinCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JoinCode",
                table: "Classes",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_JoinCode",
                table: "Classes",
                column: "JoinCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Classes_JoinCode",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "JoinCode",
                table: "Classes");
        }
    }
}
