using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineTimeTrack.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Worklogs",
                columns: table => new
                {
                    WorklogID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectID = table.Column<long>(nullable: false),
                    UserID = table.Column<long>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    EstimateWorkTime = table.Column<int>(nullable: false),
                    Feature = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worklogs", x => x.WorklogID);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectTitle = table.Column<string>(nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    WorklogID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK_Projects_Worklogs_WorklogID",
                        column: x => x.WorklogID,
                        principalTable: "Worklogs",
                        principalColumn: "WorklogID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Timelogs",
                columns: table => new
                {
                    TimelogID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorklogID = table.Column<long>(nullable: false),
                    ActualWorkTimeStart = table.Column<DateTime>(nullable: false),
                    ActualWorkTimeEnd = table.Column<DateTime>(nullable: false),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timelogs", x => x.TimelogID);
                    table.ForeignKey(
                        name: "FK_Timelogs_Worklogs_WorklogID",
                        column: x => x.WorklogID,
                        principalTable: "Worklogs",
                        principalColumn: "WorklogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Dob = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    ContactNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PasswordKey = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    WorklogID = table.Column<long>(nullable: true),
                    TimelogID1 = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Timelogs_TimelogID1",
                        column: x => x.TimelogID1,
                        principalTable: "Timelogs",
                        principalColumn: "TimelogID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Worklogs_WorklogID",
                        column: x => x.WorklogID,
                        principalTable: "Worklogs",
                        principalColumn: "WorklogID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_WorklogID",
                table: "Projects",
                column: "WorklogID");

            migrationBuilder.CreateIndex(
                name: "IX_Timelogs_WorklogID",
                table: "Timelogs",
                column: "WorklogID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TimelogID1",
                table: "Users",
                column: "TimelogID1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WorklogID",
                table: "Users",
                column: "WorklogID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Timelogs");

            migrationBuilder.DropTable(
                name: "Worklogs");
        }
    }
}
