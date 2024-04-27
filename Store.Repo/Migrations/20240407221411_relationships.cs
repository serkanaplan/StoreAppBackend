using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.Repo.Migrations
{
    /// <inheritdoc />
    public partial class relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "009f4e7c-3d4f-41d1-b3c5-c2f6e2957d2e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "10f9c375-c528-49dc-8130-028065816e46");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c58f7db-f9c7-4987-8a12-46ad2d3143d8");

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "41f6ce70-2be6-42ea-a9e7-f0b80bd6ae8a", null, "Moderator", "MODERATOR" },
                    { "b3db7bd1-b6e7-4b0f-b37e-7957f4c72598", null, "User", "USER" },
                    { "fecf8341-0baa-4cfc-82d9-f5b96f23fc81", null, "Admin", "ADMIN" }
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1,
                columns: new[] { "Author", "CategoryId", "Price", "Title" },
                values: new object[] { null, 1, 75, "Karagöz ve Hacivat" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                columns: new[] { "Author", "CategoryId", "Price", "Title" },
                values: new object[] { null, 2, 175, "Mesnevi" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 3,
                columns: new[] { "Author", "CategoryId", "Price", "Title" },
                values: new object[] { null, 1, 375, "Devlet" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Categories_CategoryId",
                table: "Books",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Categories_CategoryId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_CategoryId",
                table: "Books");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "41f6ce70-2be6-42ea-a9e7-f0b80bd6ae8a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3db7bd1-b6e7-4b0f-b37e-7957f4c72598");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fecf8341-0baa-4cfc-82d9-f5b96f23fc81");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "009f4e7c-3d4f-41d1-b3c5-c2f6e2957d2e", null, "Moderator", "MODERATOR" },
                    { "10f9c375-c528-49dc-8130-028065816e46", null, "Admin", "ADMIN" },
                    { "2c58f7db-f9c7-4987-8a12-46ad2d3143d8", null, "User", "USER" }
                });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1,
                columns: new[] { "Author", "Category", "Price", "Title" },
                values: new object[] { "mehmet", "hikaye", 2500, "deneme" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                columns: new[] { "Author", "Category", "Price", "Title" },
                values: new object[] { "mert", "roman", 2500, "deneme2" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 3,
                columns: new[] { "Author", "Category", "Price", "Title" },
                values: new object[] { "musa", "anı", 2500, "deneme3" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "Category", "Price", "Title" },
                values: new object[] { 4, "mehmet", "hikaye", 2500, "deneme" });
        }
    }
}
