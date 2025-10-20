using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurmaMaisA.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingStudentIndexCpfRAOrganizationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_Students_RA_Cpf_OrganizationId",
                table: "Students",
                columns: new[] { "RA", "Cpf", "OrganizationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_RA_Cpf_OrganizationId",
                table: "Students");

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
    }
}
