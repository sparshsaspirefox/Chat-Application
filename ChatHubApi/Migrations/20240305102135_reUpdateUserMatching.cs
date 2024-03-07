using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatHubApi.Migrations
{
    /// <inheritdoc />
    public partial class reUpdateUserMatching : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UnReadMessages",
                table: "UserGroupMatchings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UnReadMessages",
                table: "UserGroupMatchings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
