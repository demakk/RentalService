using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalService.Dal.Migrations
{
    public partial class ORderItemLinkEntityChangesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "OrderItemLinks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderItemLinks");

            migrationBuilder.DropColumn(
                name: "InitialPrice",
                table: "OrderItemLinks");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "OrderItemLinks");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId",
                table: "OrderItemLinks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderItemLinks");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualReturnDate",
                table: "OrderItemLinks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "OrderItemLinks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "OrderItemLinks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualReturnDate",
                table: "OrderItemLinks");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "OrderItemLinks");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "OrderItemLinks");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "OrderItemLinks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderItemLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "InitialPrice",
                table: "OrderItemLinks",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId",
                table: "OrderItemLinks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductCategoryId",
                table: "OrderItemLinks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OrderItemLinks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
