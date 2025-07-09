using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone_API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixCommentTextpropertyTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommnetText",
                table: "Comments",
                newName: "CommentText");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentText",
                table: "Comments",
                newName: "CommnetText");
        }
    }
}
