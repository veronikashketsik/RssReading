using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RssReading.Data.Migrations
{
    public partial class CreateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SourceDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RssItemDatas",
                columns: table => new
                {
                    Title = table.Column<string>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    SourceDataId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RssItemDatas", x => new { x.Title, x.PublishDate });
                    table.ForeignKey(
                        name: "FK_RssItemDatas_SourceDatas_SourceDataId",
                        column: x => x.SourceDataId,
                        principalTable: "SourceDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RssItemDatas_SourceDataId",
                table: "RssItemDatas",
                column: "SourceDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RssItemDatas");

            migrationBuilder.DropTable(
                name: "SourceDatas");
        }
    }
}
