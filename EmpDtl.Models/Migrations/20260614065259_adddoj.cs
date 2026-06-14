using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpDtl.Models.Migrations
{
    /// <inheritdoc />
    public partial class adddoj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Joindate",
                table: "EmpDtlDS",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Joindate",
                table: "EmpDtlDS");
        }
    }
}
