using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PexitaMVC.Migrations
{
    /// <inheritdoc />
    public partial class UserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_AspNetUsers_UserID",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Bills",
                newName: "OwnerID");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_UserID",
                table: "Bills",
                newName: "IX_Bills_OwnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_AspNetUsers_OwnerID",
                table: "Bills",
                column: "OwnerID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_AspNetUsers_OwnerID",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "OwnerID",
                table: "Bills",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_OwnerID",
                table: "Bills",
                newName: "IX_Bills_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_AspNetUsers_UserID",
                table: "Bills",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
