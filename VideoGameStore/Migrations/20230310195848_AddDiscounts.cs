using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoGameStore.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoGameVideoGame_Discount",
                columns: table => new
                {
                    DiscountsId = table.Column<int>(type: "int", nullable: false),
                    VideoGamesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoGameVideoGame_Discount", x => new { x.DiscountsId, x.VideoGamesId });
                    table.ForeignKey(
                        name: "FK_VideoGameVideoGame_Discount_VideoGameDiscounts_DiscountsId",
                        column: x => x.DiscountsId,
                        principalTable: "VideoGameDiscounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoGameVideoGame_Discount_VideoGames_VideoGamesId",
                        column: x => x.VideoGamesId,
                        principalTable: "VideoGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideoGameVideoGame_Discount_VideoGamesId",
                table: "VideoGameVideoGame_Discount",
                column: "VideoGamesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoGameVideoGame_Discount");
        }
    }
}
