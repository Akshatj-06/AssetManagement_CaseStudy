using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagementWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class Secondseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "ContactNumber", "DateCreated", "Email", "Name", "Password", "UserType" },
                values: new object[] { 1, "New India", "123123", new DateTime(2024, 11, 19, 7, 17, 32, 824, DateTimeKind.Utc).AddTicks(2972), "abc@gmail.com", "Aks", "password1", "Action" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
