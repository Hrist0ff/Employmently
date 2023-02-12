using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class ExpirationDateOfListings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Listings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Expired",
                table: "Listings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "f27d47e8-e97a-4286-9e12-2f016c7a878e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "5c5f5601-7444-4d13-947c-3b40829c0bd0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "64491c97-fb85-4cd0-9fa2-991fb3cdb54a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "9e1a968d-9ee2-42b2-9573-6157088bf04a", "0997f36c-f827-4fe4-8c94-a000023966b3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Expired",
                table: "Listings");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "9586d74f-6e04-495a-92dc-90081315c6f5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "2ba659a5-01b5-406b-ab4c-53bffd304511");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "7c9529eb-0e40-40ec-be91-d84a0f5d96c2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1id",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "75f91ea4-c24e-4afb-9e26-a442bcd0cd03", "2d3fe249-ee0c-473c-90c5-020de6ea6936" });
        }
    }
}
