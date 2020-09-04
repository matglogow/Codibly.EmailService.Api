using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Codibly.EmailService.Api.Models.Migrations
{
    [ExcludeFromCodeCoverage]
    public partial class MakeSendOnFieldNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SendOn",
                table: "Emails",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "SendOn",
                table: "Emails",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
