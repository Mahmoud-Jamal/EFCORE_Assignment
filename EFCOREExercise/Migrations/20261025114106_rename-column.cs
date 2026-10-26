    using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCOREExercise.Migrations
{
    /// <inheritdoc />
    public partial class renamecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Authors",
                newName: "authorName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "authorName",
                table: "Authors",
                newName: "Name");
        }
    }
}
