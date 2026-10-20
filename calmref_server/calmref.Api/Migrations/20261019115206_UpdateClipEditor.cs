using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace calmref_server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClipEditor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "username",
                table: "ClipEditors");

            migrationBuilder.AddColumn<string>(
                name: "clipId1",
                table: "ClipEditors",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClipEditors_clipId1",
                table: "ClipEditors",
                column: "clipId1");

            migrationBuilder.CreateIndex(
                name: "IX_ClipEditors_userId",
                table: "ClipEditors",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClipEditors_Clips_clipId1",
                table: "ClipEditors",
                column: "clipId1",
                principalTable: "Clips",
                principalColumn: "clipId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClipEditors_Users_userId",
                table: "ClipEditors",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClipEditors_Clips_clipId1",
                table: "ClipEditors");

            migrationBuilder.DropForeignKey(
                name: "FK_ClipEditors_Users_userId",
                table: "ClipEditors");

            migrationBuilder.DropIndex(
                name: "IX_ClipEditors_clipId1",
                table: "ClipEditors");

            migrationBuilder.DropIndex(
                name: "IX_ClipEditors_userId",
                table: "ClipEditors");

            migrationBuilder.DropColumn(
                name: "clipId1",
                table: "ClipEditors");

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "ClipEditors",
                type: "longtext",
                nullable: true);
        }
    }
}
