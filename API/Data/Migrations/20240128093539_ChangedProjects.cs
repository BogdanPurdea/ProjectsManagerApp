using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class ChangedProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserProject",
                columns: table => new
                {
                    AssociatedProjectsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContributorsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserProject", x => new { x.AssociatedProjectsId, x.ContributorsId });
                    table.ForeignKey(
                        name: "FK_AppUserProject_AspNetUsers_ContributorsId",
                        column: x => x.ContributorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserProject_Projects_AssociatedProjectsId",
                        column: x => x.AssociatedProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserProject_ContributorsId",
                table: "AppUserProject",
                column: "ContributorsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserProject");
        }
    }
}
