using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace calmref_server.Migrations
{
    /// <inheritdoc />
    public partial class AddUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "char(36)", nullable: false),
                    username = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    emailVerified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EmailOtps",
                columns: table => new
                {
                    EmailOtpId = table.Column<Guid>(type: "char(36)", nullable: false),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false),
                    hashedOtp = table.Column<string>(type: "longtext", nullable: false),
                    OtpExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    failedAttempts = table.Column<int>(type: "int", nullable: false),
                    LockedUntil = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailOtps", x => x.EmailOtpId);
                    table.ForeignKey(
                        name: "FK_EmailOtps_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserCredentials",
                columns: table => new
                {
                    UserCredentialId = table.Column<Guid>(type: "char(36)", nullable: false),
                    userId = table.Column<Guid>(type: "char(36)", nullable: false),
                    hashedPassword = table.Column<string>(type: "longtext", nullable: false),
                    hashedRefreshToken = table.Column<string>(type: "longtext", nullable: false),
                    RefreshTokenExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredentials", x => x.UserCredentialId);
                    table.ForeignKey(
                        name: "FK_UserCredentials_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EmailOtps_userId",
                table: "EmailOtps",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentials_userId",
                table: "UserCredentials",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailOtps");

            migrationBuilder.DropTable(
                name: "UserCredentials");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
