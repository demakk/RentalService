using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalService.Dal.Migrations
{
    public partial class hueta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBasicInfo_UserProfiles_UserProfileId",
                table: "UserBasicInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContact_UserBasicInfo_UserBasicInfoId",
                table: "UserContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserContact",
                table: "UserContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBasicInfo",
                table: "UserBasicInfo");

            migrationBuilder.RenameTable(
                name: "UserContact",
                newName: "UserContacts");

            migrationBuilder.RenameTable(
                name: "UserBasicInfo",
                newName: "UserBasicInfos");

            migrationBuilder.RenameIndex(
                name: "IX_UserContact_UserBasicInfoId",
                table: "UserContacts",
                newName: "IX_UserContacts_UserBasicInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBasicInfo_UserProfileId",
                table: "UserBasicInfos",
                newName: "IX_UserBasicInfos_UserProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserContacts",
                table: "UserContacts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBasicInfos",
                table: "UserBasicInfos",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBasicInfos_CityId",
                table: "UserBasicInfos",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBasicInfos_Cities_CityId",
                table: "UserBasicInfos",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBasicInfos_UserProfiles_UserProfileId",
                table: "UserBasicInfos",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContacts_UserBasicInfos_UserBasicInfoId",
                table: "UserContacts",
                column: "UserBasicInfoId",
                principalTable: "UserBasicInfos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBasicInfos_Cities_CityId",
                table: "UserBasicInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBasicInfos_UserProfiles_UserProfileId",
                table: "UserBasicInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContacts_UserBasicInfos_UserBasicInfoId",
                table: "UserContacts");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserContacts",
                table: "UserContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBasicInfos",
                table: "UserBasicInfos");

            migrationBuilder.DropIndex(
                name: "IX_UserBasicInfos_CityId",
                table: "UserBasicInfos");

            migrationBuilder.RenameTable(
                name: "UserContacts",
                newName: "UserContact");

            migrationBuilder.RenameTable(
                name: "UserBasicInfos",
                newName: "UserBasicInfo");

            migrationBuilder.RenameIndex(
                name: "IX_UserContacts_UserBasicInfoId",
                table: "UserContact",
                newName: "IX_UserContact_UserBasicInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBasicInfos_UserProfileId",
                table: "UserBasicInfo",
                newName: "IX_UserBasicInfo_UserProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserContact",
                table: "UserContact",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBasicInfo",
                table: "UserBasicInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBasicInfo_UserProfiles_UserProfileId",
                table: "UserBasicInfo",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContact_UserBasicInfo_UserBasicInfoId",
                table: "UserContact",
                column: "UserBasicInfoId",
                principalTable: "UserBasicInfo",
                principalColumn: "Id");
        }
    }
}
