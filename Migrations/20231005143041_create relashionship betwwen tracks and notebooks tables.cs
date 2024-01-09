using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeterReaderAPI.Migrations
{
    /// <inheritdoc />
    public partial class createrelashionshipbetwwentracksandnotebookstables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "Notebooks");

            migrationBuilder.AddColumn<int>(
                name: "NotebookId",
                table: "Tracks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Notebooks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_NotebookId",
                table: "Tracks",
                column: "NotebookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Notebooks_NotebookId",
                table: "Tracks",
                column: "NotebookId",
                principalTable: "Notebooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Notebooks_NotebookId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_NotebookId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "NotebookId",
                table: "Tracks");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "Notebooks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "Notebooks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
