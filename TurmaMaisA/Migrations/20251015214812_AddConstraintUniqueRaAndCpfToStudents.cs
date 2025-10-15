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
            migrationBuilder.CreateIndex(
                name: "IX_Students_Cpf",
                table: "Students",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_RA",
                table: "Students",
                column: "RA",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_Cpf",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_RA",
                table: "Students");
        }
    }
}
