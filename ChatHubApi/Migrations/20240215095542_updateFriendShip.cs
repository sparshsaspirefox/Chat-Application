using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatHubApi.Migrations
{
    /// <inheritdoc />
    public partial class updateFriendShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequestStatus",
                table: "FriendShips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "FriendShips");
        }
    }
}
