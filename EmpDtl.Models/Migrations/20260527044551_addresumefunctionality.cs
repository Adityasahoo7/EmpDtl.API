using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpDtl.Models.Migrations
{
    /// <inheritdoc />
    public partial class addresumefunctionality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResemefileName",
                table: "EmpDtlDS",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumePath",
                table: "EmpDtlDS",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResemefileName",
                table: "EmpDtlDS");

            migrationBuilder.DropColumn(
                name: "ResumePath",
                table: "EmpDtlDS");
        }
    }
}
