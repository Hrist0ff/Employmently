using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class companyExtendPicturesAndEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Employees",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "be6a7582-47c6-4674-841a-55019a24444e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "8917592f-8ed5-4803-9164-894596226bec");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "aa32da0f-fb88-40a4-b923-83450901ea2f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "edaa0e67-2f62-4b97-8c4e-ffade780ea7b", "2aaff141-e6af-4ca4-b3dd-85329f39af66" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employees",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Companies");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "80844bce-a7ea-4e73-8413-228f62541b76");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "53d2b304-2b3c-420e-ba48-1e74d1473443");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "a48508f8-9c00-4f50-af3c-cbe4b3142b19");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "db014743-0e6c-44b6-8694-425850973b8e", "4381610e-9c03-4464-96d5-193d606de0e8" });
        }
    }
}
