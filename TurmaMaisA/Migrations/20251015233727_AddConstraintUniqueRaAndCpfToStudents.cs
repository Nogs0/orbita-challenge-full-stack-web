using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurmaMaisA.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraintUniqueRaAndCpfToStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Students",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Enrollments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Enrollments",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Courses",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniqueCPF_Active",
                table: "Students",
                type: "varchar(255)",
                nullable: true,
                computedColumnSql: "IF(DeletedAt IS NULL, CPF, NULL)",
                stored: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Students_RA_OrganizationId",
                table: "Students",
                columns: new[] { "RA", "OrganizationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_UniqueCPF_Active_OrganizationId",
                table: "Students",
                columns: new[] { "UniqueCPF_Active", "OrganizationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_RA_OrganizationId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_UniqueCPF_Active_OrganizationId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "UniqueCPF_Active",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Courses");
        }
    }
}
