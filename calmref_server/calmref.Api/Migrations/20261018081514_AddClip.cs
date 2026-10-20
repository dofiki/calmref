using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace calmref_server.Migrations
{
    /// <inheritdoc />
    public partial class AddClip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clips",
                columns: table => new
                {
                    clipId = table.Column<Guid>(type: "char(36)", nullable: false),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true),
                    title = table.Column<string>(type: "longtext", nullable: false),
                    content = table.Column<string>(type: "longtext", nullable: false),
                    readOnly = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clips", x => x.clipId);
                    table.ForeignKey(
                        name: "FK_Clips_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Clips_userId",
                table: "Clips",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clips");
        }
    }
}
