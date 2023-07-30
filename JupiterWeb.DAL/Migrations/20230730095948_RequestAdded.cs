using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JupiterWeb.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RequestAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "JupiterTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "ReviewRequested",
                table: "JupiterTask",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "List<string>",
                columns: table => new
                {
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReviewed = table.Column<bool>(type: "bit", nullable: false),
                    TaskID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    userSentById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    userSentToId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_JupiterTask_TaskID",
                        column: x => x.TaskID,
                        principalTable: "JupiterTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_User_userSentById",
                        column: x => x.userSentById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_User_userSentToId",
                        column: x => x.userSentToId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_TaskID",
                table: "Requests",
                column: "TaskID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_userSentById",
                table: "Requests",
                column: "userSentById");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_userSentToId",
                table: "Requests",
                column: "userSentToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "List<string>");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "JupiterTask");

            migrationBuilder.DropColumn(
                name: "ReviewRequested",
                table: "JupiterTask");
        }
    }
}
