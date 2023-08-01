using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JupiterWeb.DAL.Migrations
{
    /// <inheritdoc />
    public partial class isApproved1Aug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Requests");
        }
    }
}
