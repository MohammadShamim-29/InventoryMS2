﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryMS.Migrations
{
    /// <inheritdoc />
    public partial class updateproducttableaddingsellingpricecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Products");
        }
    }
}
