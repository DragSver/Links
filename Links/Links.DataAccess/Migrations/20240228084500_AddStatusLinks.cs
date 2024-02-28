using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Links.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Links",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Links");
        }
    }
}
