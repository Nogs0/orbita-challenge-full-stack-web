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
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Enrollments",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Cpf_OrganizationId",
                table: "Students",
                columns: new[] { "Cpf", "OrganizationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_RA_OrganizationId",
                table: "Students",
                columns: new[] { "RA", "OrganizationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_Cpf_OrganizationId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_RA_OrganizationId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Enrollments");
        }
    }
}
