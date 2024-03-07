using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatHubApi.Migrations
{
    /// <inheritdoc />
    public partial class updateUserMatching : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnReadMessages",
                table: "UserGroupMatchings",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnReadMessages",
                table: "UserGroupMatchings");
        }
    }
}
