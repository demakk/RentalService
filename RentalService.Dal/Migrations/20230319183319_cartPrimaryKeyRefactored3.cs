using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalService.Dal.Migrations
{
    public partial class cartPrimaryKeyRefactored3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ItemId",
                table: "ShoppingCarts",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_UserProfileId",
                table: "ShoppingCarts",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Items_ItemId",
                table: "ShoppingCarts",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_UserProfiles_UserProfileId",
                table: "ShoppingCarts",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Items_ItemId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_UserProfiles_UserProfileId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_ItemId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_UserProfileId",
                table: "ShoppingCarts");
        }
    }
}
