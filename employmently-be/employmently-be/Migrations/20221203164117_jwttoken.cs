using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class jwttoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0",
                column: "ConcurrencyStamp",
                value: "e9a6bbed-8d57-4379-b9d3-418e49f59f1c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "2d13906e-ab31-40ba-98d4-777d98b271e2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "6e0aa116-b5fc-4821-ac3a-5d1a5400acfb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0",
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af582944-5ed7-47cc-8e6b-70129fb51e9b", "admin@gmail.com", "AQAAAAEAACcQAAAAEDH4ryHkFVgvxLG8qcv5M79tg/UHQu2BbbVzQu92kgdh0lRMHHOuO1ywdDDDldRJHg==", "5667a24a-3088-4079-ab45-893365e4a039" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "75674d85-3616-4a5d-87ea-ac3bbf9a5d25", "34ba793a-eab7-4560-a95c-2f79a94c96b0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0",
                column: "ConcurrencyStamp",
                value: "7148499b-f9cc-4537-88d9-9a448a025632");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "04e12c9f-b87c-4bbf-a7fe-4bb0dc26d069");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "9da79914-077e-45d2-9099-3314b2de57cf");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0",
                columns: new[] { "ConcurrencyStamp", "Email", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d0b41f93-cd23-4f8e-893e-a6bd5642eeab", "admin@admin.com", "admin123", "46a3ebdc-a3df-43bb-8cae-0d61151e4d5f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "19995b85-489b-457b-8ef2-dfd028132761", "e8b4bd1a-12ec-478c-a3b4-8fffba58018e" });
        }
    }
}
