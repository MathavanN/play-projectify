using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PlayProjectify.ProductService.Migrations
{
    /// <inheritdoc />
    public partial class ProductSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "Name", "Price", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new Guid("e4e9ef00-fc2b-4fde-8900-c3c69016ea18"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "6.5-inch display, 128GB storage, 5G support", "Smartphone X12", 799m, 30, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("e4e9ef00-fc2b-4fde-8900-c3c69016ea18"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "15-inch laptop with 16GB RAM and 1TB SSD", "Laptop Pro 15", 1499m, 15, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("e4e9ef00-fc2b-4fde-8900-c3c69016ea18"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Noise cancelling over-ear headphones", "Bluetooth Headphones", 199m, 50, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("e4e9ef00-fc2b-4fde-8900-c3c69016ea18"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Fitness tracking smartwatch with heart rate monitor", "Smartwatch Active", 249m, 40, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("e4e9ef00-fc2b-4fde-8900-c3c69016ea18"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Waterproof speaker with deep bass", "Portable Bluetooth Speaker", 99m, 60, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("c7f2295b-9ec3-4d52-9c0a-727350255749"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "High-speed RC car with rechargeable battery", "Remote Control Car", 59m, 25, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("c7f2295b-9ec3-4d52-9c0a-727350255749"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "500-piece creative building block set", "Building Blocks Set", 49m, 40, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("c7f2295b-9ec3-4d52-9c0a-727350255749"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Multiplayer strategy board game for ages 10+", "Board Game: Strategy Wars", 39m, 20, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("c7f2295b-9ec3-4d52-9c0a-727350255749"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Scenic landscape puzzle for adults", "Puzzle 1000 Pieces", 29m, 35, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("c7f2295b-9ec3-4d52-9c0a-727350255749"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Superhero action figure 5-pack", "Action Figure Set", 79m, 18, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new Guid("6cf79ab1-e24d-411f-a467-629807552051"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Foldable treadmill with digital display", "Treadmill Home Runner", 899m, 5, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000001"), new Guid("d61d02f9-160f-439a-8f2d-d7dc87da4d99"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "A Handbook of Agile Software Craftsmanship", "Clean Code", 45m, 100, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new Guid("d61d02f9-160f-439a-8f2d-d7dc87da4d99"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Your Journey to Mastery", "The Pragmatic Programmer", 50m, 80, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new Guid("7e42e4ab-ea84-4055-8f73-f4cefd3998fe"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "100% cotton, breathable fabric", "Men's Casual T-Shirt", 19m, 120, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("7e42e4ab-ea84-4055-8f73-f4cefd3998fe"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Classic fit blue denim jacket", "Women's Denim Jacket", 79m, 45, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("7e42e4ab-ea84-4055-8f73-f4cefd3998fe"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Lightweight shoes for everyday training", "Running Sneakers", 99m, 60, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("7e42e4ab-ea84-4055-8f73-f4cefd3998fe"), new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc), "Genuine leather slim wallet", "Leather Wallet", 35m, 75, new DateTime(2026, 2, 11, 13, 56, 0, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("40000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("50000000-0000-0000-0000-000000000004"));
        }
    }
}
