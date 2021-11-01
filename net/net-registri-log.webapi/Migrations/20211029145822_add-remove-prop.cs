using Microsoft.EntityFrameworkCore.Migrations;

namespace net_registri_log.webapi.Migrations
{
    public partial class addremoveprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "WeatherForecast",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "WeatherForecast");
        }
    }
}
