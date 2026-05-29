using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hold.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Balance = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.ID);
                    table.UniqueConstraint("AK_Account_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Transitions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Ammount = table.Column<float>(type: "REAL", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transitions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Transitions_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transitions_AccountID",
                table: "Transitions",
                column: "AccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transitions");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
