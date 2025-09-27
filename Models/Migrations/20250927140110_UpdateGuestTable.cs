﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGuestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Guests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guests_UserId",
                table: "Guests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guests_AspNetUsers_UserId",
                table: "Guests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guests_AspNetUsers_UserId",
                table: "Guests");

            migrationBuilder.DropIndex(
                name: "IX_Guests_UserId",
                table: "Guests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Guests");
        }
    }
}
