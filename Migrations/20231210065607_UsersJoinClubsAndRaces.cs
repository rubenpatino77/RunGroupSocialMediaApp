using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunGroupSocialMedia.Migrations
{
    /// <inheritdoc />
    public partial class UsersJoinClubsAndRaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserClub",
                columns: table => new
                {
                    ClubMembersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JoinedClubsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClub", x => new { x.ClubMembersId, x.JoinedClubsId });
                    table.ForeignKey(
                        name: "FK_AppUserClub_AspNetUsers_ClubMembersId",
                        column: x => x.ClubMembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserClub_Clubs_JoinedClubsId",
                        column: x => x.JoinedClubsId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRace",
                columns: table => new
                {
                    JoinedRacesId = table.Column<int>(type: "int", nullable: false),
                    RaceMembersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRace", x => new { x.JoinedRacesId, x.RaceMembersId });
                    table.ForeignKey(
                        name: "FK_AppUserRace_AspNetUsers_RaceMembersId",
                        column: x => x.RaceMembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRace_Races_JoinedRacesId",
                        column: x => x.JoinedRacesId,
                        principalTable: "Races",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserClub_JoinedClubsId",
                table: "AppUserClub",
                column: "JoinedClubsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRace_RaceMembersId",
                table: "AppUserRace",
                column: "RaceMembersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserClub");

            migrationBuilder.DropTable(
                name: "AppUserRace");
        }
    }
}
