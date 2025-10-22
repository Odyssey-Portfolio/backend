using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdysseyPortfolio_Libraries.Migrations
{
    /// <inheritdoc />
    public partial class User_AddFlag_OutOfCommentOutOfLimitApplied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OutOfCommentLimitTimeApplied",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutOfCommentLimitTimeApplied",
                table: "AspNetUsers");
        }
    }
}
