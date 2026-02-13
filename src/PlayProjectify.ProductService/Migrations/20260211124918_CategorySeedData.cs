using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlayProjectify.ProductService.Migrations
{
    /// <inheritdoc />
    public partial class CategorySeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("6cf79ab1-e24d-411f-a467-629807552051"), new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc), "Fitness equipment, outdoor gear, and sports accessories.", "Sports & Outdoors", new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc) },
                    { new Guid("7e42e4ab-ea84-4055-8f73-f4cefd3998fe"), new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc), "Clothing, shoes, and accessories for men and women.", "Fashion", new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc) },
                    { new Guid("c7f2295b-9ec3-4d52-9c0a-727350255749"), new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc), "Children’s toys, board games, and entertainment items.", "Toys & Games", new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc) },
                    { new Guid("d61d02f9-160f-439a-8f2d-d7dc87da4d99"), new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc), "Printed and digital books across all genres.", "Books", new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc) },
                    { new Guid("e4e9ef00-fc2b-4fde-8900-c3c69016ea18"), new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc), "Electronic devices and gadgets.", "Electronics", new DateTime(2026, 2, 11, 13, 49, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6cf79ab1-e24d-411f-a467-629807552051"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7e42e4ab-ea84-4055-8f73-f4cefd3998fe"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c7f2295b-9ec3-4d52-9c0a-727350255749"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d61d02f9-160f-439a-8f2d-d7dc87da4d99"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e4e9ef00-fc2b-4fde-8900-c3c69016ea18"));
        }
    }
}
