using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace calmref_server.Migrations
{
    /// <inheritdoc />
    public partial class AddClipEditor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClipEditors",
                columns: table => new
                {
                    clipEditorId = table.Column<Guid>(type: "char(36)", nullable: false),
                    clipId = table.Column<string>(type: "varchar(255)", nullable: false),
                    userId = table.Column<Guid>(type: "char(36)", nullable: true),
                    username = table.Column<string>(type: "longtext", nullable: true),
                    editedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClipEditors", x => x.clipEditorId);
                    table.ForeignKey(
                        name: "FK_ClipEditors_Clips_clipId",
                        column: x => x.clipId,
                        principalTable: "Clips",
                        principalColumn: "clipId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClipEditors_clipId",
                table: "ClipEditors",
                column: "clipId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClipEditors");
        }
    }
}
