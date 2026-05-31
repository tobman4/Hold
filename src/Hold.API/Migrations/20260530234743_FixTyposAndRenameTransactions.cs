using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hold.API.Migrations
{
    /// <inheritdoc />
    public partial class FixTyposAndRenameTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transitions_Account_AccountID",
                table: "Transitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transitions",
                table: "Transitions");

            migrationBuilder.RenameTable(
                name: "Transitions",
                newName: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Ammount",
                table: "Transactions",
                newName: "Amount");

            migrationBuilder.RenameIndex(
                name: "IX_Transitions_AccountID",
                table: "Transactions",
                newName: "IX_Transactions_AccountID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Account_AccountID",
                table: "Transactions",
                column: "AccountID",
                principalTable: "Account",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Account_AccountID",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transitions");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transitions",
                newName: "Ammount");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AccountID",
                table: "Transitions",
                newName: "IX_Transitions_AccountID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transitions",
                table: "Transitions",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transitions_Account_AccountID",
                table: "Transitions",
                column: "AccountID",
                principalTable: "Account",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
