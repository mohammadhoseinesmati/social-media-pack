using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace social_media.Migrations
{
    /// <inheritdoc />
    public partial class addpostidtonotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "message",
                table: "Notifications",
                newName: "Message");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Notifications",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Notifications",
                newName: "message");
        }
    }
}
