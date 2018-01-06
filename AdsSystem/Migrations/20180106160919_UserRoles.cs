using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AdsSystem.Migrations
{
    public partial class UserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdvertiserId",
                table: "Banners",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banners_AdvertiserId",
                table: "Banners",
                column: "AdvertiserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banners_Users_AdvertiserId",
                table: "Banners",
                column: "AdvertiserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banners_Users_AdvertiserId",
                table: "Banners");

            migrationBuilder.DropIndex(
                name: "IX_Banners_AdvertiserId",
                table: "Banners");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AdvertiserId",
                table: "Banners");
        }
    }
}
