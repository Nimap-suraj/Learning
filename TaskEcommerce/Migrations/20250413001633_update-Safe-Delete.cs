﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class updateSafeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Users");
        }
    }
}
