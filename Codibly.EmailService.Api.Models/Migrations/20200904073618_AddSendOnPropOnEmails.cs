using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Codibly.EmailService.Api.Models.Migrations
{
    public partial class AddSendOnPropOnEmails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SendOn",
                table: "Emails",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendOn",
                table: "Emails");
        }
    }
}
