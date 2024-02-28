using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatHubApi.Migrations
{
    /// <inheritdoc />
    public partial class addLastSeen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "FriendShips");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeen",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "RequestStatus",
                table: "FriendShips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
