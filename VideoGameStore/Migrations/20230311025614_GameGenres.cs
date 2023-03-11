using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoGameStore.Migrations
{
    /// <inheritdoc />
    public partial class GameGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameGenre",
                table: "VideoGames",
                newName: "GameGenres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameGenres",
                table: "VideoGames",
                newName: "GameGenre");
        }
    }
}
