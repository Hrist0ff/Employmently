using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace employmentlybe.Migrations
{
    /// <inheritdoc />
    public partial class ListingsAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryListing_Category_CategoriesId",
                table: "CategoryListing");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorID",
                table: "Listings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CompanyyId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameColumn(
                name: "AuthorID",
                table: "Listings",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_AuthorID",
                table: "Listings",
                newName: "IX_Listings_AuthorId");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Listings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "83f25997-843a-462f-b7ab-fb93863c3dd4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "838c1aa7-a572-41de-9a40-7d4ace9aa15e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "4c78b6f3-5331-4906-a9cd-1420e53327e0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "87908a23-9941-427c-b549-40238d849d73", "e8dedad1-f02e-4d75-905b-f159070fc666" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "c8112c67-314f-4c84-bd09-58c47e220bd9", "9b8feeb3-ade8-43c9-a44f-356cd7d46faa" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryListing_Categories_CategoriesId",
                table: "CategoryListing",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorId",
                table: "Listings",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryListing_Categories_CategoriesId",
                table: "CategoryListing");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorId",
                table: "Listings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Listings",
                newName: "AuthorID");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_AuthorId",
                table: "Listings",
                newName: "IX_Listings_AuthorID");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorID",
                table: "Listings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyyId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "84c08bd1-ec3e-40f3-a806-a44d7ff9ff9a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "1f017ec5-e94d-4b77-8928-b21de343001c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3",
                column: "ConcurrencyStamp",
                value: "804aca74-d672-4d85-983b-4da308cf59df");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "CompanyyId", "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { null, "98433601-8c97-4346-8a33-c950860fd3ae", "60588e69-6583-49ce-b4a4-711d01d3ca78" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "CompanyyId", "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { null, "8a095b00-0ce0-4e03-bc80-7366696157a0", "3722386e-9b09-4ee8-884e-3e6f922b4d62" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryListing_Category_CategoriesId",
                table: "CategoryListing",
                column: "CategoriesId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_AspNetUsers_AuthorID",
                table: "Listings",
                column: "AuthorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
