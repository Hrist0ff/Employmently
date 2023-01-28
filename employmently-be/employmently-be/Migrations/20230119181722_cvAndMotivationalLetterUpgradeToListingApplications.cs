using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class cvAndMotivationalLetterUpgradeToListingApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CV",
                table: "ListingApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "motivationalLetter",
                table: "ListingApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "e042d0cb-46af-4d4d-be84-7a37f6056985");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "bfba19e9-b3e6-42bd-a655-dd03f72549be");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "93a6f5aa-fecd-4aa2-8cc4-0096ef783ad8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c67f9eda-ba02-48e0-828e-52ea2524dc34", "87ae75e9-650b-470d-9fb2-c47a93ce60ee" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CV",
                table: "ListingApplications");

            migrationBuilder.DropColumn(
                name: "motivationalLetter",
                table: "ListingApplications");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "b9603ea5-8dcd-4fab-a221-c46d6d7de77c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "2dab0fec-62e0-4be5-8eae-3211a302e0a0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "bc702354-4586-42d7-9bae-6f4c90ee5f56");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "efefde91-94d0-485f-bd8a-45cfd2d3aa28", "7085b04a-a94b-497a-b666-29f2d2ba23e2" });
        }
    }
}
