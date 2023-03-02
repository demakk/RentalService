using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalService.Dal.Migrations
{
    public partial class orderItemLinkChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemLinks_OrderItemLinks_ItemId",
                table: "OrderItemLinks");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemLinks_Items_ItemId",
                table: "OrderItemLinks",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemLinks_Items_ItemId",
                table: "OrderItemLinks");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemLinks_OrderItemLinks_ItemId",
                table: "OrderItemLinks",
                column: "ItemId",
                principalTable: "OrderItemLinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
