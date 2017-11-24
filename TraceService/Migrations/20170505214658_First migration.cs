using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TraceService.Migrations
{
    public partial class Firstmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Traces",
                columns: table => new
                {
                    TraceId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CorrelationId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Module = table.Column<string>(nullable: true),
                    Object = table.Column<string>(nullable: true),
                    ObjectId = table.Column<string>(nullable: true),
                    Operation = table.Column<string>(nullable: true),
                    Origin = table.Column<string>(maxLength: 255, nullable: false),
                    TraceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traces", x => x.TraceId);
                });

            migrationBuilder.CreateTable(
                name: "TraceOrigins",
                columns: table => new
                {
                    OriginId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Origin = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraceOrigins", x => x.OriginId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Traces");

            migrationBuilder.DropTable(
                name: "TraceOrigins");
        }
    }
}
