using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillSnap.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_PortfolioUsers_PortfolioUserId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_PortfolioUsers_PortfolioUserId",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioUsers",
                table: "PortfolioUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "PortfolioUsers",
                newName: "PortfolioUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioUser",
                table: "PortfolioUser",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileName = table.Column<string>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelBadge = table.Column<string>(type: "TEXT", nullable: false),
                    PrestigeLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    PrestigeBadge = table.Column<string>(type: "TEXT", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_PortfolioUser_PortfolioUserId",
                table: "Projects",
                column: "PortfolioUserId",
                principalTable: "PortfolioUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_PortfolioUser_PortfolioUserId",
                table: "Skills",
                column: "PortfolioUserId",
                principalTable: "PortfolioUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_PortfolioUser_PortfolioUserId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_PortfolioUser_PortfolioUserId",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PortfolioUser",
                table: "PortfolioUser");

            migrationBuilder.RenameTable(
                name: "PortfolioUser",
                newName: "PortfolioUsers");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PortfolioUsers",
                table: "PortfolioUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_PortfolioUsers_PortfolioUserId",
                table: "Projects",
                column: "PortfolioUserId",
                principalTable: "PortfolioUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_PortfolioUsers_PortfolioUserId",
                table: "Skills",
                column: "PortfolioUserId",
                principalTable: "PortfolioUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
