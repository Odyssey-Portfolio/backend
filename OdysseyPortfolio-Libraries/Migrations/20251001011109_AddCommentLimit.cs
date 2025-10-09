using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdysseyPortfolio_Libraries.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfCommentsLeft",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfCommentsLeft",
                table: "AspNetUsers");
        }
    }
}
