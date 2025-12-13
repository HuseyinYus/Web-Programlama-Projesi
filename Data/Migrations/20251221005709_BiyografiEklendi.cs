using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Programlama_Projesi.Data.Migrations
{
    /// <inheritdoc />
    public partial class BiyografiEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "Trainers",
                newName: "Biography");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Biography",
                table: "Trainers",
                newName: "Bio");
        }
    }
}
